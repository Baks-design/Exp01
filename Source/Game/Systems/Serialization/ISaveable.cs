namespace Game.Systems.Serialization;

/// <summary>
/// An object that can save and restore its state.
/// State is passed as raw JSON so the storage layer stays serializer-agnostic.
/// </summary>
public interface ISaveable
{
    /// <summary>Unique key used as the dictionary entry in the save file.</summary>
    string SaveKey { get; }

    /// <summary>Serializes this object's state to a JSON string.</summary>
    string CaptureState();
    /// <summary>Deserializes the given JSON string back into this object.</summary>
    void RestoreState(string state);
}