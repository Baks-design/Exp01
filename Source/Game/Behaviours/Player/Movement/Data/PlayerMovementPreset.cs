using FlaxEngine;

namespace Game.Behaviours.Player;

public sealed class PlayerMovementPreset
{
    [Header("First Person")]
    public float FPSensitivity = 1;
    public float FPSmoothing = 20;
    public float FPJumpForce = 800;
    public float FPGravityMultiplier = 2.5f;
    public float FPJumpBufferTime = 0.1f;
    public float FPCoyoteTime = 0.1f;
    public float FPSpeed = 400;
    public float FPRunSpeed = 400;
    public float FPAcceleration = 10;
    public float FPDeceleration = 10;
    public float FPRotationLerpSpeed = 15;
    public float FPFriction = 8.0f;
    public float FPGroundAccelerate = 5000;
    public float FPAirAccelerate = 10000;
    public float FPMaxVelocityGround = 400;
    public float FPMaxVelocityAir = 200;

    [Header("Third Person")]
    public float Speed = 250;
    public float SprintSpeed = 300;
    public float GravityMultiplier = 2f;
    public float JumpStrength = 10;
    public float MouseSensitivity = 100;
    public float CameraLag = 10;
    public float FOVZoom = 50;
    public Float2 PitchMinMax = new(-45, 45);
}
