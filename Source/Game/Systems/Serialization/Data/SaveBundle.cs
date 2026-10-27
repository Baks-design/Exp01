using System;

namespace Game.Systems.Serialization;

[Serializable]
public sealed class SaveBundle
{
    public PlayerSaveData Player;
}