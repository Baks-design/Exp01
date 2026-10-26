using FlaxEngine;

namespace Game.Behaviours.Player;

public sealed class FirstPersonMovementPreset
{
    [Tooltip("Speed at which the camera position/orientation is smoothed toward the camera target. Higher = snappier. Only used when camera smoothing is enabled.")]
    public float cameraSmoothing = 20;
    [Tooltip("Initial upward velocity applied on jump, in cm/s. Jump height ≈ jumpForce² / (2 * |gravity| * gravityMultiplier). 450 with gravityMultiplier=2 gives ~52 cm.")]
    public float jumpForce = 450;
    [Tooltip("Multiplier applied to |Physics.Gravity.Y| for manual vertical integration. Keep AutoGravity OFF on the controller when using this.")]
    public float gravityMultiplier = 2;
    [Tooltip("How long a jump press is remembered before landing, in seconds.")]
    public float jumpBufferTime = 0.1f;
    [Tooltip("How long after leaving the ground the player can still jump, in seconds.")]
    public float coyoteTime = 0.1f;
    [Tooltip("How quickly the controller turns to match the camera yaw. Higher = faster.")]
    public float rotationLerpSpeed = 15;
    [Tooltip("Ground friction rate (per second). Higher = stops faster.")]
    public float friction = 8;
    [Tooltip("Ground acceleration in cm/s². Higher = snappier starts/stops.")]
    public float groundAccelerate = 5000;
    [Tooltip("Air acceleration in cm/s². Keep much lower than groundAccelerate for limited air control.")]
    public float airAccelerate = 2500;
    [Tooltip("Maximum horizontal ground speed in cm/s.")]
    public float maxVelocityGround = 400;
    [Tooltip("Maximum air acceleration speed in cm/s. Does not clamp total air speed.")]
    public float maxVelocityAir = 200;
    [Tooltip("Length of the downward ground-check ray in cm, cast from the bottom of the capsule.")]
    public float groundCheck = 10;
    [Tooltip("Layers considered walkable ground.")]
    public LayersMask groundLayers = LayersMask.Default;
    [Tooltip("Frames after a jump during which the ground-stick force is suppressed.")]
    public int jumpGraceFrames = 2;
    [Tooltip("Distance in cm between footstep sounds. ~200 cm ≈ 2 m.")]
    public float footstepDistance = 200;
}
