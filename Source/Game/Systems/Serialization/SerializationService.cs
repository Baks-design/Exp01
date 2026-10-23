using System.Collections.Generic;

namespace Game.Systems.Serialization;

// Use: 
// serializationService = new SaveManager(new FileSaveStorage("save.json"))
// serializationService.SaveAll();
// serializationService.LoadAll();
public sealed class SerializationService(ISaveStorage storage)
{
    readonly Dictionary<string, ISaveable> saveables = [];
    readonly ISaveStorage storage = storage;

    public void Register(ISaveable saveable) => saveables[saveable.SaveKey] = saveable;

    public void SaveAll()
    {
        var bundle = new Dictionary<string, object>();
        foreach (var kvp in saveables) bundle[kvp.Key] = kvp.Value.CaptureState();
        storage.Write(bundle);
    }

    public void LoadAll()
    {
        var bundle = storage.Read();
        foreach (var kvp in bundle)
            if (saveables.TryGetValue(kvp.Key, out var saveable))
                saveable.RestoreState(kvp.Value);
    }
}