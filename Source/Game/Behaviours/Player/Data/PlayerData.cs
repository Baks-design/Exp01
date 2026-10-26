using FlaxEngine;
using FlaxEngine.Json;
using Game.Systems.Serialization;

namespace Game.Behaviours.Player;

public class PlayerData : Script, ISaveable
{
    public string SaveKey => "Player";

    public string CaptureState()
    {
        var data = new PlayerSaveData
        {
            Position = Actor.Position
        };
        return JsonSerializer.Serialize(data);
    }

    public void RestoreState(string state)
    {
        var data = JsonSerializer.Deserialize<PlayerSaveData>(state);
        if (data == null) return;

        Actor.Position = data.Position;
    }
}