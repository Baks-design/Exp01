using FlaxEngine;

namespace Game.Behaviours.Player;

public struct MovementResult
{
    public Vector3 Displacement;
    public CharacterMoveState State;
    public float SpeedRatio;
}
