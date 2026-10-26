using System;

namespace Game.Systems.Serialization;

/// <summary>
/// Optional strongly-typed save container. Use this if you want a single
/// monolithic save with compile-time field checking instead of the keyed
/// <see cref="SerializationService"/> approach.
/// </summary>
[Serializable]
public sealed class SaveBundle
{
    public PlayerSaveData Player;
}