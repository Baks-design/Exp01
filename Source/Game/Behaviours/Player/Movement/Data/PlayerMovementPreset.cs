using FlaxEngine;

namespace Game.Behaviours.Player;

public sealed class PlayerMovementPreset
{
    [Header("First Person")]
    public float FPSpeed = 600;
    public float FPRunSpeed = 1200;
    public float FPJumpForce = 400;
    public float FPGravityMultiplier = 2;
    public float FPRotationLerpSpeed = 10;
    public float FPAcceleration = 12;     
    public float FPDeceleration = 16;      
    public float FPCoyoteTime = 0.12f;
    public float FPJumpBufferTime = 0.15f;
    [Header("Third Person")]
    public float TPSpeed = 600;
    public float TPRunSpeed = 1200;
    public float TPJumpForce = 400;
    public float TPGravityMultiplier = 2;
    public float TPRotationLerpSpeed = 10;
    public float TPAcceleration = 12;
    public float TPDeceleration = 16;
    public float TPCoyoteTime = 0.12f;
    public float TPJumpBufferTime = 0.15f;
}
