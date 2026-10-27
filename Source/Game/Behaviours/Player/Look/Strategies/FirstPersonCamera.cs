using FlaxEngine;
using Game.Core.Extensions;
using Game.Systems.Inputs;

namespace Game.Behaviours.Player;

public sealed class FirstPersonCamera : ICameraStrategy
{
    FirstPersonCameraPreset preset;

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

        ctx.cameraData.yaw += mouseDelta.X * preset.sensitivity;
        ctx.cameraData.pitch += mouseDelta.Y * preset.sensitivity;

        ctx.cameraData.pitch = Mathf.Clamp(ctx.cameraData.pitch, preset.minMaxPitch.X, preset.minMaxPitch.Y);

        var t = 1 - Mathf.Exp(-preset.lookSmoothing * ctx.deltaTime);

        ctx.cameraData.smoothedYaw = Mathf.LerpAngle(ctx.cameraData.smoothedYaw, ctx.cameraData.yaw, t);
        ctx.cameraData.smoothedPitch = Mathf.Lerp(ctx.cameraData.smoothedPitch, ctx.cameraData.pitch, t);

        ctx.cameraYaw.LocalOrientation = Quaternion.Euler(0, ctx.cameraData.smoothedYaw, 0);
        ctx.cameraPitch.LocalOrientation = Quaternion.Euler(ctx.cameraData.smoothedPitch, 0, 0);
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