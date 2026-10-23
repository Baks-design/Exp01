using FlaxEngine;
using Game.Core.Extensions;
using Game.Systems.Input;

namespace Game.Behaviours.Player;

public sealed class FirstPersonCameraStrategy : ICameraStrategy
{
    FirstPersonCameraPreset preset;

    public JsonAssetReference<FirstPersonCameraPreset> PresetReference { get; set; }

    public void SetPreset(FirstPersonCameraPreset preset) => this.preset = preset;

    public void Execute(CameraContext ctx)
    {
        if (preset == null) return;

        Rotation(ctx);
        FOVZoom(ctx);
        ApplyShake(ctx);
    }

    void Rotation(CameraContext ctx)
    {
        var mouseDelta = new Float2(ctx.input.GetAxis(GameAxis.LookX), ctx.input.GetAxis(GameAxis.LookY));
        var ySign = preset.invertY ? -1 : 1;

        var targetYaw = ctx.cameraData.yaw + mouseDelta.X * preset.sensitivity;
        var targetPitch = ctx.cameraData.pitch + mouseDelta.Y * preset.sensitivity * ySign;

        var t = 1 - Mathf.Exp(-preset.lookSmoothing * Time.DeltaTime);

        ctx.cameraData.yaw = Mathf.LerpAngle(ctx.cameraData.yaw, targetYaw, t);
        var toClamped = Mathf.Clamp(targetPitch, preset.minMaxPitch.X, preset.minMaxPitch.Y);
        ctx.cameraData.pitch = Mathf.Lerp(ctx.cameraData.pitch, toClamped, t);

        ctx.cameraTarget.LocalOrientation = Quaternion.Euler(ctx.cameraData.pitch, ctx.cameraData.yaw, 0);
    }

    void FOVZoom(CameraContext ctx)
    {
        if (!ctx.cameraData.initialized)
        {
            ctx.cameraData.defaultFov = ctx.camera.FieldOfView;
            ctx.cameraData.initialized = true;
        }

        var targetFov = ctx.input.GetAction(InputActions.Aim) ? preset.fOVZoom : ctx.cameraData.defaultFov;
        var t = preset.fOVLerpSpeed * Time.DeltaTime;
        ctx.camera.FieldOfView = ctx.camera.FieldOfView.Eerp(targetFov, t);
    }

    void ApplyShake(CameraContext ctx)
    {
        if (ctx.cameraData.shakeTimer <= 0 || ctx.cameraData.shakeIntensity <= 0) return;

        ctx.cameraData.shakeTimer -= Time.DeltaTime;
        var decay = Mathf.Exp(-preset.shakeDecay * Time.DeltaTime);
        ctx.cameraData.shakeIntensity *= decay;

        if (ctx.cameraData.shakeTimer <= 0 || ctx.cameraData.shakeIntensity < 0.001f)
        {
            ctx.cameraData.shakeIntensity = 0;
            ctx.cameraData.shakeTimer = 0;
            return;
        }

        var t = Time.GameTime + ctx.cameraData.shakeSeed;
        var offset = new Vector3(
            Mathf.Sin(t * 37.1f) * ctx.cameraData.shakeIntensity,
            Mathf.Sin(t * 47.3f) * ctx.cameraData.shakeIntensity, 0);

        ctx.camera.LocalPosition += offset * preset.shakeScale;
    }
}