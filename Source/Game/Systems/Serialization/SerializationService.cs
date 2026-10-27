using System;
using System.Collections.Generic;
using FlaxEngine;
using FlaxEngine.Json;

namespace Game.Systems.Serialization;

public sealed class SerializationService(ISaveStorage storage)
{
    readonly Dictionary<string, ISaveable> saveables = [];
    readonly ISaveStorage storage = storage ?? throw new ArgumentNullException(nameof(storage));

    public void Register(ISaveable saveable)
    {
        ArgumentNullException.ThrowIfNull(saveable);
        saveables[saveable.SaveKey] = saveable;
    }

    public void Unregister(string saveKey) => saveables?.Remove(saveKey);

    public void SaveAll()
    {
        var bundle = new Dictionary<string, string>();
        foreach (var kvp in saveables) bundle[kvp.Key] = kvp.Value.CaptureState();
        var json = JsonSerializer.Serialize(bundle);
        storage.Write(json);
    }

    public void LoadAll()
    {
        var json = storage.Read();
        if (string.IsNullOrEmpty(json)) return;

        Dictionary<string, string> bundle;
        try
        {
            bundle = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
        }
        catch (Exception ex)
        {
            Debug.LogError($"SerializationService.LoadAll: failed to parse save file: {ex}");
            return;
        }

        if (bundle == null) return;

        foreach (var kvp in bundle)
        {
            if (saveables.TryGetValue(kvp.Key, out var saveable))
            {
                try
                {
                    saveable.RestoreState(kvp.Value);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"SerializationService.LoadAll: failed to restore '{kvp.Key}': {ex}");
                }
            }
            else
                Debug.LogWarning($"SerializationService.LoadAll: no saveable registered for key '{kvp.Key}'.");
        }
    }
}