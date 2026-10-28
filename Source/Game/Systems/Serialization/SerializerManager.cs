using System.IO;
using FlaxEngine;

namespace Game.Systems.Serialization;

public sealed class SerializerManager : GamePlugin
{
    public static SerializerManager Instance => PluginManager.GetPlugin<SerializerManager>();
    public SerializationService Serializer { get; private set; }
    public GameSettings Settings { get; private set; }

    public SerializerManager() => _description = new PluginDescription
    {
        Name = "Serializer Manager",
        Description = "Global save/load service.",
        Category = "Gameplay"
    };

    public override void Initialize()
    {
        base.Initialize();
        var savePath = Path.Combine(Globals.ProductLocalFolder, "save.json");
        var storage = new FileSaveStorage(savePath);
        Serializer = new SerializationService(storage);
        Settings = new GameSettings();
    }

    public override void Deinitialize()
    {
        Serializer = null;
        Settings = null;
        base.Deinitialize();
    }

    public void SaveEntities() => Serializer?.SaveAll();
    public void LoadEntities() => Serializer?.LoadAll();
    public void SaveSettings() => Settings?.Save();
    public void LoadSettings() => Settings?.Load();
}