using FlaxEngine;
using Game.Systems.Serialization;

namespace Game.Behaviours.Player;

public sealed class PlayerData : Script, ISaveable
{
    public string SaveKey => "Player";

    public object CaptureState() => new PlayerSaveData { Position = Actor.Position };

    public void RestoreState(object state)
    {
        var data = (PlayerSaveData)state;
        Actor.Position = data.Position;
    }
}