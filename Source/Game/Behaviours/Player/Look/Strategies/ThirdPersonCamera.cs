using FlaxEngine;
using Game.Systems.Inputs;

namespace Game.Behaviours.Player;

public sealed class ThirdPersonCamera : ICameraStrategy
{
    ThirdPersonCameraPreset preset;
    float currentDistance = -1f;
    bool wasPausedLastFrame;

    public JsonAssetReference<ThirdPersonCameraPreset> PresetReference { get; set; }

    public void SetPreset(ThirdPersonCameraPreset preset) => this.preset = preset;

    public void Configure()
    {
        currentDistance = -1f;
        if (PresetReference && PresetReference.Asset != null)
            preset = PresetReference.Asset.CreateInstance<ThirdPersonCameraPreset>();
        else
        {
            if (PresetReference) Debug.LogWarning("ThirdPersonCamera: PresetReference set but asset failed to load.");
            preset = null;
        }
    }

    public void Execute(CameraContext ctx)
    {
        if (preset == null) return;

        Rotation(ctx);
        ApplySpringArm(ctx);
        FOVZoom(ctx);
    }

    void Rotation(CameraContext ctx)
    {
        var mouseDelta = new Float2(ctx.input.GetAxis(GameAxis.LookX), ctx.input.GetAxis(GameAxis.LookY));

        var pitchSign = preset.invertY ? 1f : -1f;

        ctx.cameraData.yaw += mouseDelta.X * preset.sensitivity;
        ctx.cameraData.pitch += mouseDelta.Y * preset.sensitivity * pitchSign;

        var range = CameraMath.NormalizePitchRange(preset.minMaxPitch);
        ctx.cameraData.pitch = Mathf.Clamp(ctx.cameraData.pitch, range.X, range.Y);

        var t = CameraMath.ExpSmooth(preset.smoothing, ctx.deltaTime);

        ctx.cameraData.smoothedYaw = Mathf.LerpAngle(ctx.cameraData.smoothedYaw, ctx.cameraData.yaw, t);
        ctx.cameraData.smoothedPitch = Mathf.Lerp(ctx.cameraData.smoothedPitch, ctx.cameraData.pitch, t);

        ctx.cameraYaw.LocalOrientation = Quaternion.Euler(0f, ctx.cameraData.smoothedYaw, 0f);
        ctx.cameraPitch.LocalOrientation = Quaternion.Euler(ctx.cameraData.smoothedPitch, 0f, 0f);
    }

    void ApplySpringArm(CameraContext ctx)
    {
        var lookRot = Quaternion.Euler(ctx.cameraData.smoothedPitch, ctx.cameraData.smoothedYaw, 0f);
        var camDir = Vector3.Transform(Vector3.Backward, lookRot).Normalized;

        var desiredDistance = preset.distance;
        var pivotPos = ctx.cameraTarget.Position;

        var targetDistance = desiredDistance;
        if (Physics.RayCast(pivotPos, camDir, out var hit, desiredDistance + preset.collisionBias,
            preset.cameraCollisionLayers, false))
            targetDistance = Mathf.Max(hit.Distance - preset.collisionBias, preset.minDistance);

        if (currentDistance < 0f) currentDistance = targetDistance;
        else
        {
            var t = CameraMath.ExpSmooth(preset.collisionSmooth, ctx.deltaTime);
            currentDistance = Mathf.Lerp(currentDistance, targetDistance, t);
        }

        ctx.camera.Position = pivotPos + camDir * currentDistance;
        ctx.camera.Orientation = Quaternion.LookRotation(-camDir, Vector3.Up);
    }

    void FOVZoom(CameraContext ctx) =>
        CameraMath.ApplyFovZoom(ctx, ctx.input.GetAction(InputActions.Aim), preset.fOVZoom, preset.fOVLerpSpeed);
}