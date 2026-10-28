using FlaxEngine;

namespace Game.Behaviours.Player;

public sealed class ThirdPersonMovementPreset
{
    [Tooltip("Speed at which the camera orientation is smoothed toward its target. " +
             "Higher = snappier camera, lower = floatier. Only affects pitch-flattening " +
             "of the camera's local orientation; yaw follows the camera actor directly.")]
    public float cameraSmoothing = 20f;

    [Tooltip("Initial upward velocity applied on jump, in cm/s. " +
             "Jump height ≈ jumpForce² / (2 * |gravity| * gravityMultiplier). " +
             "800 with gravityMultiplier=2 gives ~163 cm (~1.6 m) of jump height.")]
    public float jumpForce = 800f;

    [Tooltip("Multiplier applied to |Physics.Gravity.Y| for manual vertical integration. " +
             "Keep AutoGravity OFF on the CharacterController when using this. " +
             "2 = double gravity for a snappier, more game-like fall.")]
    public float gravityMultiplier = 2f;

    [Tooltip("How long a jump press is remembered before landing, in seconds. " +
             "Lets the player queue a jump slightly before touching the ground. " +
             "0.1 s is a good default; raise for more forgiving platforming.")]
    public float jumpBufferTime = 0.1f;

    [Tooltip("How long after leaving the ground the player can still jump, in seconds. " +
             "Covers small ledges and slopes where IsGrounded briefly reports false. " +
             "0.1 s is a good default; raise for more forgiving platforming.")]
    public float coyoteTime = 0.1f;

    [Tooltip("How quickly the controller turns to face its target direction. " +
             "Higher = faster/snappier turn, lower = smoother/floatier. " +
             "Used for both strafe (face camera yaw) and non-strafe (face move direction).")]
    public float rotationLerpSpeed = 15f;

    [Tooltip("Ground friction rate (per second). Higher = the player stops faster " +
             "when releasing input on the ground. Typical range: 4–12.")]
    public float friction = 8f;

    [Tooltip("Ground acceleration in cm/s². Higher = snappier starts and stops. " +
             "Must be large relative to maxVelocityGround to feel responsive " +
             "(5000 reaches 400 cm/s in ~0.08 s).")]
    public float groundAccelerate = 5000f;

    [Tooltip("Air acceleration in cm/s². Controls how much the player can steer mid-air. " +
             "Lower than groundAccelerate for limited air control; " +
             "higher for floaty/expressive movement. Third-person games often use 5000–15000.")]
    public float airAccelerate = 10000f;

    [Tooltip("Maximum horizontal ground speed in cm/s. " +
             "This is a projection cap (Quake-style accelerate), not a hard clamp — " +
             "external impulses can exceed it until friction bleeds them off.")]
    public float maxVelocityGround = 400f;

    [Tooltip("Maximum air acceleration speed in cm/s. " +
             "This caps the *acceleration projection* per axis, NOT the total air speed. " +
             "External impulses can exceed it; momentum is preserved.")]
    public float maxVelocityAir = 200f;

    [Tooltip("When ON, the character always faces the camera yaw (strafing, " +
             "action-RPG style). When OFF, the character faces its horizontal " +
             "movement direction (platformer / action-adventure style).")]
    public bool strafe = false;

    [Tooltip("Frames after a jump during which the downward ground-stick force " +
             "is suppressed. Prevents the controller from being pulled back to the " +
             "floor on the first frames of a jump. 2 frames is usually enough.")]
    public int jumpGraceFrames = 2;

    [Tooltip("Downward force (cm/s²) applied when grounded but not jumping, " +
             "to keep the character glued to slopes and stairs. " +
             "~490 (half of Earth gravity at 981) works well; too high may cause " +
             "sticking on steep slopes, too low may let the player slide off.")]
    public float stickyGroundForce = 490f;

    [Tooltip("Layers considered walkable ground for the ground-check ray. " +
             "Must include the layers of all floors the player should stand on. " +
             "Exclude the player's own layer to avoid self-hits.")]
    public LayersMask groundLayers = LayersMask.Default;

    [Tooltip("Length of the downward ground-check ray in cm, cast from the bottom " +
             "of the capsule. Used as a fallback when CharacterController.IsGrounded " +
             "is false (e.g. small ledges, stepping off colliders). " +
             "10 cm is a good default; raise on rough terrain.")]
    public float groundCheck = 10f;

    [Tooltip("Distance in cm between footstep sounds while moving on the ground. " +
             "~200 cm ≈ 2 m, matching a typical human stride. " +
             "Lower for faster/denser footsteps, higher for sparse ones.")]
    public float footstepDistance = 200f;
}