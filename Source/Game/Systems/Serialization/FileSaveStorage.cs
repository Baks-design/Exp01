using System;
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
            AtomicFile.Write(filePath, json);
        }
        catch (Exception ex)
        {
            Debug.LogError($"FileSaveStorage.Write failed for '{filePath}': {ex.Message}");
        }
    }

    public string Read() => AtomicFile.Read(filePath);
}
