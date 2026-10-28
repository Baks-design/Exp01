using FlaxEngine;

namespace Game.Behaviours.Player;

public sealed class ThirdPersonCameraPreset
{
    [Tooltip("Mouse look sensitivity multiplier applied to raw mouse delta. Higher = faster camera rotation. Note: this is NOT scaled by delta time because mouse delta is already per-frame displacement.")]
    public float sensitivity = 0.15f;

    [Tooltip("Minimum and maximum pitch (look up/down) in degrees. X = min, Y = max. Order is normalized at runtime, so (60, -60) behaves the same as (-60, 60).")]
    public Float2 minMaxPitch = new(-60f, 60f);

    [Tooltip("Target distance (in world units) the spring arm pushes the camera back from the pivot when unobstructed.")]
    public float distance = 400f;

    [Tooltip("Closest distance (in world units) the spring arm is allowed to pull the camera in when a collision is detected. Prevents the camera from clipping into the player.")]
    public float minDistance = 80f;

    [Tooltip("Extra distance (in world units) subtracted from the collision hit point so the camera stops slightly before touching geometry. Also extends the raycast length by this amount to detect nearby obstacles early.")]
    public float collisionBias = 10f;

    [Tooltip("LayersMask the spring-arm raycast tests against. Include world geometry and obstacles the camera should not pass through. Leave empty to disable collision entirely.")]
    public LayersMask cameraCollisionLayers = LayersMask.Default;

    [Tooltip("Field of View (in degrees) used while the Aim action is held. Should be less than the camera's default FOV for a zoom effect. Valid range: 1-179.")]
    public float fOVZoom = 50f;

    [Tooltip("Speed at which the camera's Field of View lerps toward the current target FOV. Higher = faster zoom transitions. Uses exponential smoothing, so it is framerate-independent.")]
    public float fOVLerpSpeed = 8f;

    [Tooltip("Look-rotation smoothing speed, in units of 1/second. Higher = snappier; lower = floatier. Uses exponential smoothing, so it is framerate-independent. 0 disables smoothing entirely.")]
    public float smoothing = 10f;

    [Tooltip("Spring-arm distance smoothing speed, in units of 1/second. Higher = snappier collision recovery; lower = softer collision response. Uses exponential smoothing, so it is framerate-independent.")]
    public float collisionSmooth = 20f;

    [Tooltip("Invert vertical mouse look. When true, moving the mouse up looks down.")]
    public bool invertY = false;
}