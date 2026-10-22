using FlaxEngine;
using Game.Systems.Input;

namespace Game.Behaviours.Player;

public sealed class ThirdPersonCameraStrategy : ICameraStrategy //TODO: Better Smooth
{
    public JsonAssetReference<CameraPreset> PresetReference { get; set; }

    public void ApplyLook(InputService input, Actor cameraTarget, CameraData cameraData, CameraPreset preset, Camera camera)
    {
        Rotation(input, cameraTarget, cameraData, preset);
        FOVZoom(input, cameraData, preset, camera);
    }

    static void Rotation(InputService input, Actor cameraTarget, CameraData cameraData, CameraPreset preset)
    {
        cameraData.yaw += input.GetAxis(GameAxis.LookX) * preset.MouseSensitivity * Time.DeltaTime;
        cameraData.pitch += input.GetAxis(GameAxis.LookY) * preset.MouseSensitivity * Time.DeltaTime;
        cameraData.pitch = Mathf.Clamp(cameraData.pitch, preset.PitchMinMax.X, preset.PitchMinMax.Y);

        var target = Quaternion.Euler(cameraData.pitch, cameraData.yaw, 0);
        cameraTarget.Orientation = Quaternion.Lerp(cameraTarget.Orientation, target, Time.DeltaTime * preset.CameraLag);
    }

    static void FOVZoom(InputService input, CameraData cameraData, CameraPreset preset, Camera camera)
    {
        if (!cameraData.initialized)
        {
            cameraData.defaultFov = camera.FieldOfView;
            cameraData.initialized = true;
        }
        var targetFov = input.GetAction(InputActions.Aim) ? preset.FOVZoom : cameraData.defaultFov;
        camera.FieldOfView = Mathf.Lerp(camera.FieldOfView, targetFov, Time.DeltaTime * 5f);
    }
}