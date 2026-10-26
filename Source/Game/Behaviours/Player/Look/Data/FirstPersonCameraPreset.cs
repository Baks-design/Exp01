using FlaxEngine;

namespace Game.Behaviours.Player;

public sealed class FirstPersonCameraPreset
{
    [Tooltip("Invert vertical mouse look. When true, moving the mouse up looks down.")]
    public bool invertY = false;
    [Tooltip("Smoothing speed for look rotation, in units of 1/second. Higher = snappier; lower = floatier. 0 disables smoothing entirely.")]
    public float lookSmoothing = 15;
    [Tooltip("Minimum and maximum pitch (look up/down) in degrees. X = min, Y = max. Order is normalized at runtime, so (60, -60) behaves the same as (-60, 60).")]
    public Float2 minMaxPitch = new(-60, 60);
    [Tooltip("Mouse look sensitivity multiplier applied to raw mouse delta. Higher = faster camera. Note: this is NOT scaled by delta time because mouse delta is already per-frame displacement.")]
    public float sensitivity = 2;
    [Tooltip("Speed at which the camera's Field of View lerps toward the current target FOV. Higher = faster zoom transitions. Uses exponential smoothing, so it is framerate-independent.")]
    public float fOVLerpSpeed = 10;
    [Tooltip("Field of View (in degrees) used while the Aim action is held. Should be less than the camera's default FOV for a zoom effect. Valid range: 1-179.")]
    public float fOVZoom = 45;
}