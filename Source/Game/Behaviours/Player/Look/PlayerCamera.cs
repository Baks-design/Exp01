using FlaxEngine;
using Game.Systems.Input;

namespace Game.Behaviours.Player;

public sealed class PlayerCamera : Script
{
    [Header("Strategies")]
    public ThirdPersonCameraStrategy thirdPerson = new();
    public FirstPersonCameraStrategy firstPerson = new();
    [Header("References")]
    public Camera camera;
    public Actor cameraTarget;
    public Actor lockOnTarget;
    readonly InputService input = new();
    readonly CameraData data = new();
    object activePreset;
    ICameraStrategy activeStrategy;

    public override void OnStart()
    {
        if (!camera)
        {
            Debug.LogError("PlayerCamera: Camera is not assigned.");
            return;
        }
        if (!cameraTarget)
        {
            Debug.LogError("PlayerCamera: CameraTarget is not assigned.");
            return;
        }
    }

    public override void OnUpdate()
        => activeStrategy.ApplyLook(input, cameraTarget, data, activePreset, camera, this);

    public void SetCameraStrategy<T>(ICameraStrategy<T> newStrategy) where T : class
    {
        if (newStrategy == null || activeStrategy == newStrategy) return;

        activeStrategy = newStrategy;
        data.Reset();

        var reference = newStrategy.PresetReference;
        if (reference == null || !reference.Asset)
        {
            Debug.LogError($"{newStrategy.GetType().Name} has no preset assigned.");
            activePreset = null;
            return;
        }

        activePreset = reference.Asset.CreateInstance<T>();
    }

    public void SetShake(float intensity, float duration)
        => activeStrategy.TriggerShake(data, intensity, duration);

    public void SetRotation(float pitch, float yaw)
        => activeStrategy.AddMovementRotation(data, pitch, yaw);
}