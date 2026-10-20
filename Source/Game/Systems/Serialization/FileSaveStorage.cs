using System.Collections.Generic;
using System.IO;
using FlaxEngine.Json;

namespace Game.Systems.Serialization;

public sealed class FileSaveStorage(string filePath) : ISaveStorage
{
    private readonly string filePath = filePath;

    public void Write(Dictionary<string, object> data)
    {
        var json = JsonSerializer.Serialize(data);
        File.WriteAllText(filePath, json);
    }

    public Dictionary<string, object> Read()
    {
        if (!File.Exists(filePath)) return [];
        
        var json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<Dictionary<string, object>>(json);
    }
}
