using FlaxEngine;
using Game.Systems.Input;

namespace Game.Behaviours.Player;

public sealed class ThirdPersonCameraStrategy : ICameraStrategy
{
    Vector3 currentPos;

    public JsonAssetReference<CameraPreset> PresetReference { get; set; }

    public void Update(CameraData data, InputService input, Camera camera, Actor target, CameraPreset preset)
    {
        Rotation(data, input, preset, out var rotation);
        Position(camera, target, preset, rotation);
    }

    static void Rotation(CameraData data, InputService input, CameraPreset preset, out Quaternion rotation)
    {
        var mouseDelta = InputService.GetMousePosition;

        data.yaw += mouseDelta.X * preset.TPMouseSpeed;
        data.pitch = Mathf.Clamp(data.yaw, preset.TPMinPitch, preset.TPMaxPitch);

        rotation = Quaternion.Euler(-data.pitch, data.yaw, 0);
    }

    void Position(Camera camera, Actor target, CameraPreset preset, Quaternion rotation)
    {
        var targetPos = target.Position;
        var direction = Vector3.Transform(Vector3.Forward, rotation).Normalized;

        var targetDistance = preset.TPDistance;
        if (Physics.RayCast(targetPos, direction, out var hit, preset.TPDistance, preset.TPCollisionLayers))
        {
            var bias = preset.TPCollisionDistanceBias + camera.NearPlane;
            targetDistance = Mathf.Max(hit.Distance - bias, 10);
        }

        var idealPos = targetPos + direction * targetDistance;

        var t = 1 - Mathf.Exp(-preset.TPSmoothing * Time.DeltaTime);
        currentPos = Vector3.Lerp(currentPos, idealPos, t);

        camera.Position = currentPos;
        camera.LookAt(targetPos, Vector3.Up);
    }
}