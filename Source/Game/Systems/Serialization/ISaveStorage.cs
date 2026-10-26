namespace Game.Systems.Serialization;

/// <summary>
/// Abstraction over a persistent storage backend.
/// Implementations must be atomic and must not throw on missing files.
/// </summary>
public interface ISaveStorage
{
    /// <summary>Writes the raw JSON string to storage atomically.</summary>
    void Write(string json);
    /// <summary>
    /// Reads the raw JSON string from storage.
    /// Returns null if nothing has been saved yet.
    /// </summary>
    string Read();
}