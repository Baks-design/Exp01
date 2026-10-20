using FlaxEngine;

namespace Game.Behaviours.Player;

public sealed class PlayerMovementPreset
{
    [Header("Movement")]
    public float Speed = 600f;
    public float JumpForce = 400f;
    public float GravityMultiplier = 2f;
    public float RotationLerpSpeed = 10f;
    [Header("Forgiveness")]
    public float CoyoteTime = 0.12f;
    public float JumpBufferTime = 0.15f;
}
