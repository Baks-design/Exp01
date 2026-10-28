using System;
using System.IO;
using FlaxEngine;

namespace Game.Systems.Serialization;

public static class AtomicFile
{
    public static void Write(string path, string contents)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException("path cannot be null or empty.", nameof(path));
        ArgumentNullException.ThrowIfNull(contents);

        var dir = Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        var tempPath = path + ".tmp";
        try
        {
            File.WriteAllText(tempPath, contents);

            if (File.Exists(path))
            {
                try
                {
                    File.Replace(tempPath, path, destinationBackupFileName: null);
                }
                catch (IOException)
                {
                    File.Move(tempPath, path, overwrite: true);
                }
            }
            else File.Move(tempPath, path);
        }
        catch (Exception ex)
        {
            Debug.LogError($"AtomicFile.Write failed for '{path}': {ex}");
            TryDelete(tempPath);
            throw;
        }
    }

    public static string Read(string path)
    {
        try
        {
            return File.Exists(path) ? File.ReadAllText(path) : null;
        }
        catch (Exception ex)
        {
            Debug.LogError($"AtomicFile.Read failed for '{path}': {ex}");
            return null;
        }
    }

    static void TryDelete(string path)
    {
        try { if (File.Exists(path)) File.Delete(path); }
        catch { }
    }
}