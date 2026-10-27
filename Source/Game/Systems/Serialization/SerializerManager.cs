using System.IO;
using FlaxEngine;
using Game.Behaviours.Player;

namespace Game.Systems.Serialization;

public sealed class SerializerManager
{
    SerializationService serializer;
    GameSettings settings;

    public void CreateSave()
    {
        var storage = new FileSaveStorage(Path.Combine(Globals.ProductLocalFolder, "save.json"));
        serializer = new(storage);
        var player = Level.FindScript<PlayerData>();
        serializer?.Register(player);
    }

    public void CreateSettings()
    {
        settings = new();
        settings?.Save();
    }

    public void SaveEntities() => serializer?.SaveAll();

    public void SaveSettings() => settings?.Save();

    public void LoadEntities() => serializer?.LoadAll();

    public void LoadSettings() => settings?.Load();
}