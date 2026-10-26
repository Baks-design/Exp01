using FlaxEngine;
using Game.Core.Extensions;
using Game.Systems.Inputs;

namespace Game.Behaviours.Player;

public sealed class FirstPersonCamera : ICameraStrategy
{
    FirstPersonCameraPreset preset;
    float targetYaw;
    float targetPitch;

    public JsonAssetReference<FirstPersonCameraPreset> PresetReference { get; set; }

    public void SetPreset(FirstPersonCameraPreset preset) => this.preset = preset;

    public void Execute(CameraContext ctx)
    {
        if (preset == null) return;

        ctx.deltaTime = Time.DeltaTime;
        Rotation(ctx);
        FOVZoom(ctx);
    }

    void Rotation(CameraContext ctx)
    {
        var mouseDelta = new Float2(ctx.input.GetAxis(GameAxis.LookX), ctx.input.GetAxis(GameAxis.LookY));
        var sensY = preset.invertY ? -preset.sensitivity : preset.sensitivity;
        
        targetYaw += mouseDelta.X * preset.sensitivity;
        targetPitch += mouseDelta.Y * sensY;
        targetPitch = Mathf.Clamp(targetPitch, preset.minMaxPitch.X, preset.minMaxPitch.Y);

        var t = 1 - Mathf.Exp(-preset.lookSmoothing * ctx.deltaTime);
        ctx.cameraData.yaw = Mathf.LerpAngle(ctx.cameraData.yaw, targetYaw, t);
        ctx.cameraData.pitch = Mathf.Lerp(ctx.cameraData.pitch, targetPitch, t);

        ctx.camera.LocalOrientation = Quaternion.Euler(ctx.cameraData.pitch, ctx.cameraData.yaw, 0);
    }

    void FOVZoom(CameraContext ctx)
    {
        if (!ctx.cameraData.initialized)
        {
            ctx.cameraData.defaultFov = ctx.camera.FieldOfView;
            ctx.cameraData.initialized = true;
        }

        var aimFov = Mathf.Clamp(preset.fOVZoom, 1, 179);
        var targetFov = ctx.input.GetAction(InputActions.Aim) ? aimFov : ctx.cameraData.defaultFov;

        var t = 1 - Mathf.Exp(-preset.fOVLerpSpeed * ctx.deltaTime);
        ctx.camera.FieldOfView = ctx.camera.FieldOfView.Eerp(targetFov, t);
    }
}