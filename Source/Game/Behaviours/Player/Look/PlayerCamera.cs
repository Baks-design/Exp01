using FlaxEngine;
using Game.Core.Cursor;
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
    readonly InputService input = new();
    readonly CameraData data = new();
    CameraPreset activePreset;
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
    {
        CursorService.Apply(CursorState.Gameplay);
        activeStrategy.ApplyLook(input, cameraTarget, data, activePreset, camera);
    }

    public void SetCameraStrategy(ICameraStrategy newStrategy)
    {
        if (newStrategy == null || activeStrategy == newStrategy) return;

        activeStrategy = newStrategy;
        data.Reset();
        RefreshActivePreset(newStrategy);
    }

    void RefreshActivePreset(ICameraStrategy newStrategy)
    {
        var reference = newStrategy.PresetReference;
        if (reference == null)
        {
            activePreset = null;
            return;
        }
        else
            activePreset = reference.Instance;
    }
}