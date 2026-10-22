using FlaxEngine;

namespace Game.Behaviours.Player;

public sealed class PlayerMovementPreset //TODO: Separate
{
    [Header("First Person")]
    public float FPCameraSmoothing = 20;
    public float FPJumpForce = 800;
    public float FPGravityMultiplier = 2;
    public float FPJumpBufferTime = 0.1f;
    public float FPCoyoteTime = 0.1f;
    public float FPRotationLerpSpeed = 15;
    public float FPFriction = 8;
    public float FPGroundAccelerate = 5000;
    public float FPAirAccelerate = 10000;
    public float FPMaxVelocityGround = 400;
    public float FPMaxVelocityAir = 200;
    public float FPGroundCheck = 10;
    public LayersMask FPGroundLayers = LayersMask.Default;
    public int FPJumpGraceFrames = 2;  
    public float FPFootstepDistance = 2;

    [Header("Third Person")]
    public float TPCameraSmoothing = 20;
    public float TPJumpForce = 800;
    public float TPGravityMultiplier = 2;
    public float TPJumpBufferTime = 0.1f;
    public float TPCoyoteTime = 0.1f;
    public float TPRotationLerpSpeed = 15;
    public float TPFriction = 8;
    public float TPGroundAccelerate = 5000;
    public float TPAirAccelerate = 10000;
    public float TPMaxVelocityGround = 400;
    public float TPMaxVelocityAir = 200;
    public bool TPStrafe = false;   
    public int TPJumpGraceFrames = 2;       
    public float TPStickyGroundForce = 490;
    public LayersMask TPGroundLayers = LayersMask.Default;
    public float TPGroundCheck = 10;
}
