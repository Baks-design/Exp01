using FlaxEngine;
using Game.Systems.Serialization;

namespace Game.Behaviours.Player;

public sealed class PlayerData : Script, ISaveable //FIXME: 
{
    public CharacterController controller;
    public string SaveKey => "Player";

    public object CaptureState() => new PlayerSaveData { Position = controller.Position };

    public void RestoreState(object state)
    {
        var data = (PlayerSaveData)state;
        controller.Position = data.Position;
    }
}