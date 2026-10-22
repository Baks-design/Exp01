using FlaxEngine;
using Game.Systems.Input;

namespace Game.Behaviours.Player;

public sealed class FirstPersonCameraStrategy : ICameraStrategy
{
    public JsonAssetReference<CameraPreset> PresetReference { get; set; }

    public void ApplyLook(InputService input, Actor cameraTarget, CameraData data, CameraPreset preset, Camera camera)
    {
        var mouseDelta = new Float2(input.GetAxis(GameAxis.LookX), input.GetAxis(GameAxis.LookY));
        data.pitch = Mathf.Clamp(data.pitch + mouseDelta.Y * preset.FPSensitivity, -88, 88);
        data.yaw += mouseDelta.X * preset.FPSensitivity;
        cameraTarget.LocalOrientation = Quaternion.Euler(data.pitch, data.yaw, 0); //TODO: Apply Smooth
    }
}
