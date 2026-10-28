using FlaxEngine;

namespace Game.Behaviours.Player;

public sealed class FirstPersonMovementPreset
{
    [Tooltip("Initial upward velocity applied on jump, in cm/s.")]
    public float jumpForce = 450f;

    [Tooltip("Multiplier applied to |Physics.Gravity.Y| for manual vertical integration.")]
    public float gravityMultiplier = 2f;

    [Tooltip("How long a jump press is remembered before landing, in seconds.")]
    public float jumpBufferTime = 0.1f;

    [Tooltip("How long after leaving the ground the player can still jump, in seconds.")]
    public float coyoteTime = 0.1f;

    [Tooltip("How quickly the controller turns to match the camera yaw.")]
    public float rotationLerpSpeed = 15f;

    [Tooltip("Ground friction rate (per second).")]
    public float friction = 8f;

    [Tooltip("Ground acceleration in cm/s².")]
    public float groundAccelerate = 5000f;

    [Tooltip("Air acceleration in cm/s².")]
    public float airAccelerate = 2500f;

    [Tooltip("Maximum horizontal ground speed in cm/s.")]
    public float maxVelocityGround = 400f;

    [Tooltip("Maximum air acceleration speed in cm/s.")]
    public float maxVelocityAir = 200f;

    [Tooltip("Length of the downward ground-check ray in cm.")]
    public float groundCheck = 10f;

    [Tooltip("Layers considered walkable ground.")]
    public LayersMask groundLayers = LayersMask.Default;

    [Tooltip("Frames after a jump during which the ground-stick force is suppressed.")]
    public int jumpGraceFrames = 2;

    [Tooltip("Distance in cm between footstep sounds.")]
    public float footstepDistance = 200f;
}