using FlaxEngine;
using Game.Systems.Inputs;

namespace Game.Behaviours.Player;

public sealed class FirstPersonCamera : ICameraStrategy
{
    FirstPersonCameraPreset preset;

    public JsonAssetReference<FirstPersonCameraPreset> PresetReference { get; set; }

    public void SetPreset(FirstPersonCameraPreset preset) => this.preset = preset;

    public void Configure()
    {
        if (PresetReference && PresetReference.Asset != null)
            preset = PresetReference.Asset.CreateInstance<FirstPersonCameraPreset>();
        else
        {
            if (PresetReference)
                Debug.LogWarning("FirstPersonCamera: PresetReference set but asset failed to load.");
            preset = null;
        }
    }

    public void Execute(CameraContext ctx)
    {
        if (preset == null) return;
    
        Rotation(ctx);
        FOVZoom(ctx);
    }

    void Rotation(CameraContext ctx)
    {
        var mouseDelta = new Float2(ctx.input.GetAxis(GameAxis.LookX), ctx.input.GetAxis(GameAxis.LookY));

        var pitchSign = preset.invertY ? -1f : 1f;

        ctx.cameraData.yaw += mouseDelta.X * preset.sensitivity;
        ctx.cameraData.pitch += mouseDelta.Y * preset.sensitivity * pitchSign;

        var range = CameraMath.NormalizePitchRange(preset.minMaxPitch);
        ctx.cameraData.pitch = Mathf.Clamp(ctx.cameraData.pitch, range.X, range.Y);

        var t = CameraMath.ExpSmooth(preset.lookSmoothing, ctx.deltaTime);

        ctx.cameraData.smoothedYaw = Mathf.LerpAngle(ctx.cameraData.smoothedYaw, ctx.cameraData.yaw, t);
        ctx.cameraData.smoothedPitch = Mathf.Lerp(ctx.cameraData.smoothedPitch, ctx.cameraData.pitch, t);

        ctx.cameraYaw.LocalOrientation = Quaternion.Euler(0f, ctx.cameraData.smoothedYaw, 0f);
        ctx.cameraPitch.LocalOrientation = Quaternion.Euler(ctx.cameraData.smoothedPitch, 0f, 0f);
    }

    void FOVZoom(CameraContext ctx)
        => CameraMath.ApplyFovZoom(ctx, ctx.input.GetAction(InputActions.Aim), preset.fOVZoom, preset.fOVLerpSpeed);
}