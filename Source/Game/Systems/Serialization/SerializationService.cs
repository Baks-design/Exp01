using System;
using System.Collections.Generic;
using FlaxEngine;
using FlaxEngine.Json;

namespace Game.Systems.Serialization;

public sealed class SerializationService(ISaveStorage storage)
{
    const string VersionKey = "__version";
    const int CurrentVersion = 1;

    readonly Dictionary<string, ISaveable> saveables = [];
    readonly ISaveStorage storage = storage ?? throw new ArgumentNullException(nameof(storage));

    public void Register(ISaveable saveable)
    {
        ArgumentNullException.ThrowIfNull(saveable);

        if (string.IsNullOrWhiteSpace(saveable.SaveKey))
            throw new ArgumentException("SaveKey must not be null or empty.", nameof(saveable));

        if (saveables.ContainsKey(saveable.SaveKey))
            Debug.LogWarning($"SerializationService.Register: overwriting SaveKey '{saveable.SaveKey}'.");

        saveables[saveable.SaveKey] = saveable;
    }

    public void Unregister(string saveKey)
    {
        if (string.IsNullOrEmpty(saveKey)) return;
        saveables.Remove(saveKey);
    }

    public void SaveAll()
    {
        var bundle = new Dictionary<string, string>(saveables.Count + 1)
        {
            [VersionKey] = CurrentVersion.ToString()
        };

        foreach (var kvp in saveables)
        {
            try
            {
                bundle[kvp.Key] = kvp.Value.CaptureState() ?? string.Empty;
            }
            catch (Exception ex)
            {
                Debug.LogError($"SerializationService.SaveAll: failed to capture '{kvp.Key}': {ex}");
            }
        }

        try
        {
            var json = JsonSerializer.Serialize(bundle);
            storage.Write(json);
        }
        catch (Exception ex)
        {
            Debug.LogError($"SerializationService.SaveAll: failed to serialize/write: {ex}");
        }
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
        if (bundle.TryGetValue(VersionKey, out var verStr) &&
            int.TryParse(verStr, out var ver) && ver > CurrentVersion)
            Debug.LogWarning($"SerializationService.LoadAll: save version {ver} is newer than {CurrentVersion}; attempting best-effort load.");

        foreach (var kvp in bundle)
        {
            if (kvp.Key == VersionKey) continue;

            if (!saveables.TryGetValue(kvp.Key, out var saveable))
            {
#if DEBUG
                    Debug.LogWarning($"SerializationService.LoadAll: no saveable registered for key '{kvp.Key}'.");
#endif
                continue;
            }

            try
            {
                saveable.RestoreState(kvp.Value);
            }
            catch (Exception ex)
            {
                Debug.LogError($"SerializationService.LoadAll: failed to restore '{kvp.Key}': {ex}");
            }
        }
    }
}