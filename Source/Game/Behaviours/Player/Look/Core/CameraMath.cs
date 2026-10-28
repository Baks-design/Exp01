using FlaxEngine;
using Game.Core.Extensions;
using Game.Systems.Inputs;

namespace Game.Behaviours.Player;

public static class CameraMath
{
    public static float ExpSmooth(float speed, float dt) => 1f - Mathf.Exp(-speed * dt);

    public static Float2 NormalizePitchRange(Float2 range)
    {
        var lo = Mathf.Min(range.X, range.Y);
        var hi = Mathf.Max(range.X, range.Y);
        return new Float2(lo, hi);
    }

    public static void ApplyFovZoom(CameraContext ctx, bool aiming, float aimFov, float lerpSpeed)
    {
        if (!ctx.cameraData.initialized)
        {
            ctx.cameraData.defaultFov = ctx.camera.FieldOfView;
            ctx.cameraData.initialized = true;
        }

        var clampedAim = Mathf.Clamp(aimFov, 1f, 179f);
        var target = aiming ? clampedAim : ctx.cameraData.defaultFov;

        var t = ExpSmooth(lerpSpeed, ctx.deltaTime);
        ctx.camera.FieldOfView = ctx.camera.FieldOfView.Eerp(target, t);
    }

    public static void ApplyLookRotation(CameraContext ctx, Float2 minMaxPitch, float smoothing,
        bool invertY, bool subtractPitch)
    {
        var mouseDelta = new Float2(ctx.input.GetAxis(GameAxis.LookX), ctx.input.GetAxis(GameAxis.LookY));

        var pitchSign = (invertY ? -1f : 1f) * (subtractPitch ? -1f : 1f);

        ctx.cameraData.yaw += mouseDelta.X * 1f;
        ctx.cameraData.pitch += mouseDelta.Y * pitchSign;

        var range = NormalizePitchRange(minMaxPitch);
        ctx.cameraData.pitch = Mathf.Clamp(ctx.cameraData.pitch, range.X, range.Y);

        var t = ExpSmooth(smoothing, ctx.deltaTime);

        ctx.cameraData.smoothedYaw = Mathf.LerpAngle(ctx.cameraData.smoothedYaw, ctx.cameraData.yaw, t);
        ctx.cameraData.smoothedPitch = Mathf.Lerp(ctx.cameraData.smoothedPitch, ctx.cameraData.pitch, t);

        ctx.cameraYaw.LocalOrientation = Quaternion.Euler(0f, ctx.cameraData.smoothedYaw, 0f);
        ctx.cameraPitch.LocalOrientation = Quaternion.Euler(ctx.cameraData.smoothedPitch, 0f, 0f);
    }
}