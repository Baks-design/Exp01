using FlaxEngine;
using Game.Systems.Input;

namespace Game.Behaviours.Player;

public sealed class FirstPersonCameraStrategy : ICameraStrategy
{
    public JsonAssetReference<CameraPreset> PresetReference { get; set; }

    public void Update(CameraData data, InputService input, Camera camera, Actor target, CameraPreset preset)
    {
        Position(camera, target, preset);
        Rotation(data, input, camera, preset);
    }

    static void Position(Camera camera, Actor target, CameraPreset preset)
    {
        var eyePosition = target.Position + Vector3.Up * preset.FPHeightOffset;
        camera.Position = eyePosition;
    }

    static void Rotation(CameraData data, InputService input, Camera camera, CameraPreset preset)
    {
        var mouseDelta = InputService.GetMousePosition;

        data.yaw += mouseDelta.X * preset.FPRotationSpeed;
        data.pitch = Mathf.Clamp((data.pitch - mouseDelta.Y) * preset.FPRotationSpeed, preset.FPMinPitch, preset.FPMaxPitch);

        var targetRotation = Quaternion.Euler(data.pitch, data.yaw, 0);

        var t = 1 - Mathf.Exp(-preset.FPSmoothing * Time.DeltaTime);
        camera.Orientation = Quaternion.Lerp(camera.Orientation, targetRotation, t);
    }
}
