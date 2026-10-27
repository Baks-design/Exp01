using FlaxEngine;
using Game.Systems.Inputs;

namespace Game.Behaviours.Player;

public sealed class PlayerCamera : Script
{
    [Header("References")]
    public Camera camera;
    public Actor cameraPitch;
    public Actor cameraYaw;
    [Header("Strategies")]
    public FirstPersonCamera firstPerson = new();
    public ThirdPersonCamera thirdPerson = new();
    readonly InputService input = new();
    readonly CameraContext ctx = new();
    ICameraStrategy currentStrategy;

    [ReadOnly] public CameraData CameraData { get; set; } = new();

    public override void OnStart()
    {
        if (camera == null) camera = Level.FindActor<Camera>();

        ctx.input = input;
        ctx.camera = camera;
        ctx.cameraPitch = cameraPitch;
        ctx.cameraYaw = cameraYaw;
        ctx.cameraData = CameraData;
    }

    public override void OnUpdate() => currentStrategy.Execute(ctx);

    public void SetCameraStrategy(ICameraStrategy newStrategy)
    {
        if (currentStrategy == newStrategy) return;

        currentStrategy = newStrategy;
        CameraData.Reset();

        switch (newStrategy)
        {
            case FirstPersonCamera fp:
                fp.SetPreset(fp.PresetReference
                    ? fp.PresetReference.Asset?.CreateInstance<FirstPersonCameraPreset>()
                    : null);
                break;
            case ThirdPersonCamera tp:
                tp.SetPreset(tp.PresetReference
                    ? tp.PresetReference.Asset?.CreateInstance<ThirdPersonCameraPreset>()
                    : null);
                break;
        }
    }

    public void SetRotation(float pitch, float yaw)
    {
        CameraData.pitch += pitch;
        CameraData.yaw += yaw;
    }
}