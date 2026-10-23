using FlaxEngine;
using Game.Core.Extensions;
using Game.Systems.Input;

namespace Game.Behaviours.Player;

public sealed class ThirdPersonCameraStrategy : ICameraStrategy
{
    ThirdPersonCameraPreset preset;

    public JsonAssetReference<ThirdPersonCameraPreset> PresetReference { get; set; }

    public void SetPreset(ThirdPersonCameraPreset preset) => this.preset = preset;

    public void Execute(CameraContext ctx)
    {
        if (preset == null) return;

        Rotation(ctx);
        ApplySpringArm(ctx);
        FOVZoom(ctx);
        ApplyShake(ctx);
    }

    void Rotation(CameraContext ctx)
    {
        var mouseDelta = new Float2(ctx.input.GetAxis(GameAxis.LookX), ctx.input.GetAxis(GameAxis.LookY));

        var t = 1 - Mathf.Exp(-preset.lockOnLerpSpeed * Time.DeltaTime);

        ctx.cameraData.yaw = Mathf.LerpAngle(ctx.cameraData.yaw, mouseDelta.Y, t);
        var toClamped = Mathf.Clamp(mouseDelta.X, preset.minMaxPitch.X, preset.minMaxPitch.Y);
        ctx.cameraData.pitch = Mathf.Lerp(ctx.cameraData.pitch, toClamped, t);

        ctx.cameraTarget.LocalOrientation = Quaternion.Euler(ctx.cameraData.pitch, ctx.cameraData.yaw, 0);
    }

    void ApplySpringArm(CameraContext ctx)
    {
        var targetPos = ctx.cameraTarget.Position;
        var lookRot = Quaternion.Euler(ctx.cameraData.pitch, ctx.cameraData.yaw, 0);
        var desiredDir = Vector3.Transform(Vector3.Backward, lookRot).Normalized;
        var desiredDistance = preset.distance;

        Vector3 basePosition;
        if (Physics.RayCast(targetPos, desiredDir, out var hit, desiredDistance, preset.cameraCollisionLayers, false))
        {
            var safeDistance = Mathf.Max(hit.Distance - preset.collisionBias - ctx.camera.NearPlane, preset.minDistance);
            basePosition = targetPos + desiredDir * safeDistance;
        }
        else
            basePosition = targetPos + desiredDir * desiredDistance;

        ctx.camera.Position = basePosition;
        ctx.camera.Orientation = Quaternion.LookRotation(-desiredDir, Vector3.Up);
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
        ctx.cameraData.shakeIntensity *= Mathf.Exp(-preset.shakeDecay * Time.DeltaTime);

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

        ctx.camera.Position = ctx.cameraData.basePosition + offset * preset.shakeScale;
    }
}