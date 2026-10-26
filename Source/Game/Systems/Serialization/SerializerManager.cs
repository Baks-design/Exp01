using System.IO;
using FlaxEngine;
using Game.Behaviours.Player;

namespace Game.Systems.Serialization;

public class SerializerManager
{
    SerializationService serializer;
    GameSettings settings;

    public void Update()
    {
        if (Input.GetKeyDown(KeyboardKeys.F5)) serializer?.SaveAll();
        if (Input.GetKeyDown(KeyboardKeys.F9)) serializer?.LoadAll();
    }

    public void CreateSave()
    {
        var storage = new FileSaveStorage(Path.Combine(Globals.ProductLocalFolder, "save.json"));
        serializer = new SerializationService(storage);
        var player = Level.FindScript<PlayerData>();
        if (player != null) serializer.Register(player);
    }

    public void CreateSettings()
    {
        settings = new GameSettings();
        settings.Save();
    }
}