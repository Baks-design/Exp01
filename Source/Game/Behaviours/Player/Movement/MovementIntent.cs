using FlaxEngine;

namespace Game.Behaviours.Player.Movement;

public readonly record struct MovementIntent
{
    public Vector3 Direction { get; init; }
    public bool JumpPressed { get; init; }
    public bool SprintHeld { get; init; }

    public static MovementIntent Idle => default;
}
