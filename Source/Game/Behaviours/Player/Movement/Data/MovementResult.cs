using FlaxEngine;

namespace Game.Behaviours.Player;

public struct MovementResult
{
    public Vector3 TargetVelocity;   
    public CharacterMoveState State;
    public float SpeedRatio;
}
