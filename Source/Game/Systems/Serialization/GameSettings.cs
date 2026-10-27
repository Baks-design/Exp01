using System;
using System.Collections.Generic;
using System.IO;
using FlaxEngine;
using FlaxEngine.Json;

namespace Game.Systems.Serialization;

public sealed class GameSettings
{
    readonly string path;
    Dictionary<string, string> data;

    public IReadOnlyDictionary<string, string> Data => data;

    public GameSettings()
    {
        path = Path.Combine(Globals.ProductLocalFolder, "Game.settings");
        data = [];
        Load();
    }

    public string Get(string key, string defaultValue = null)
        => data.TryGetValue(key, out var v) ? v : defaultValue;

    public void Set(string key, string value)
    {
        ArgumentNullException.ThrowIfNull(key);
        data[key] = value;
    }

    public void Load()
    {
        if (!File.Exists(path)) return;

        try
        {
            var json = File.ReadAllText(path);
            var loaded = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            if (loaded != null) data = loaded;
        }
        catch (Exception ex)
        {
            Debug.LogError($"GameSettings.Load failed: {ex}");
        }
    }

    public void Save()
    {
        try
        {
            var dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            var json = JsonSerializer.Serialize(data);
            var tempPath = path + ".tmp";
            File.WriteAllText(tempPath, json);
            if (File.Exists(path))
                File.Replace(tempPath, path, null);
            else
                File.Move(tempPath, path);
        }
        catch (Exception ex)
        {
            Debug.LogError($"GameSettings.Save failed: {ex}");
        }
    }
}