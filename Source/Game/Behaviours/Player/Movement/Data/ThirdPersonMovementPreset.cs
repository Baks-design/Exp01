using FlaxEngine;

namespace Game.Behaviours.Player;

public sealed class ThirdPersonMovementPreset 
{
    public float cameraSmoothing = 20;
    public float jumpForce = 800;
    public float gravityMultiplier = 2;
    public float jumpBufferTime = 0.1f;
    public float coyoteTime = 0.1f;
    public float rotationLerpSpeed = 15;
    public float friction = 8;
    public float groundAccelerate = 5000;
    public float airAccelerate = 10000;
    public float maxVelocityGround = 400;
    public float maxVelocityAir = 200;
    public bool strafe = false;
    public int jumpGraceFrames = 2;
    public float stickyGroundForce = 490;
    public LayersMask groundLayers = LayersMask.Default;
    public float groundCheck = 10;
}
