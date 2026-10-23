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
    readonly InputService input = new();
    readonly CameraData data = new();
    readonly CameraContext ctx = new();
    ICameraStrategy strategy;

    public override void OnStart()
    {
        if (!camera) { Debug.LogError("PlayerCamera: camera missing."); return; }
        if (!cameraTarget) { Debug.LogError("PlayerCamera: cameraTarget missing."); return; }

        ctx.input = input;
        ctx.cameraTarget = cameraTarget;
        ctx.camera = camera;
        ctx.cameraData = data;
    }

    public override void OnUpdate()
    {
        if (strategy == null) return;
       
        ctx.deltaTime = Time.DeltaTime;
        strategy.Execute(ctx);
    }

    public void SetCameraStrategy(ICameraStrategy newStrategy)
    {
        if (newStrategy == null || strategy == newStrategy) return;

        strategy = newStrategy;
        data.Reset();
        switch (newStrategy)
        {
            case FirstPersonCameraStrategy fp:
                fp.SetPreset(fp.PresetReference
                    ? fp.PresetReference.Asset?.CreateInstance<FirstPersonCameraPreset>()
                    : null);
                break;
            case ThirdPersonCameraStrategy tp:
                tp.SetPreset(tp.PresetReference
                    ? tp.PresetReference.Asset?.CreateInstance<ThirdPersonCameraPreset>()
                    : null);
                break;
        }
    }

    public void SetRotation(float pitch, float yaw)
    {
        data.pitch += pitch;
        data.yaw += yaw;
    }
}