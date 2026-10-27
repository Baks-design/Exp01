using System;
using System.IO;
using FlaxEngine;

namespace Game.Systems.Serialization;

public sealed class FileSaveStorage : ISaveStorage
{
    readonly string filePath;

    public FileSaveStorage(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("filePath cannot be null or empty.", nameof(filePath));
        this.filePath = filePath;
    }

    public void Write(string json)
    {
        ArgumentNullException.ThrowIfNull(json);

        try
        {
            var dir = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) 
                Directory.CreateDirectory(dir);

            var tempPath = filePath + ".tmp";
            File.WriteAllText(tempPath, json);
            if (File.Exists(filePath))
                File.Replace(tempPath, filePath, null);
            else
                File.Move(tempPath, filePath);
        }
        catch (Exception ex)
        {
            Debug.LogError($"FileSaveStorage.Write failed for '{filePath}': {ex}");
        }
    }

    public string Read()
    {
        try
        {
            if (!File.Exists(filePath)) return null;
            return File.ReadAllText(filePath);
        }
        catch (Exception ex)
        {
            Debug.LogError($"FileSaveStorage.Read failed for '{filePath}': {ex}");
            return null;
        }
    }
}