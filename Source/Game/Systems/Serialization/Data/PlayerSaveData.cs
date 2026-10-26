using System;
using FlaxEngine;

namespace Game.Systems.Serialization;

/// <summary>
/// Plain data container for player state. Kept separate from the
/// Script that owns it so it can be serialized independently.
/// </summary>
[Serializable]
public sealed class PlayerSaveData
{
    public Vector3 Position;
}