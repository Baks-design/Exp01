using FlaxEngine;
using Game.Systems.Input;

namespace Game.Behaviours.Player;

public sealed class ThirdPersonCameraStrategy : ICameraStrategy
{
    public JsonAssetReference<CameraPreset> PresetReference { get; set; }

    public void Update(
        InputService inputService, Camera camera, Actor targetActor,
        CameraPreset activePreset, ref Vector2 mouseAngles)
    {
        mouseAngles += inputService.GetDelta * activePreset.RotationSpeed;
        mouseAngles.Y = Mathf.Clamp(mouseAngles.Y, -89f, 89f);

        var distance = activePreset.Distance + Input.MouseScrollDelta * -20f * activePreset.ZoomSpeed;
        distance = Mathf.Clamp(distance, activePreset.MinDistance, activePreset.MaxDistance);

        var rotation = Quaternion.Euler(-mouseAngles.Y, mouseAngles.X, 0f);
        var targetPosition = targetActor.Position;
        var direction = Vector3.Transform(Vector3.Forward, rotation);

        camera.Position = targetPosition + direction * distance;
        camera.LookAt(targetPosition, Vector3.Up);

        if (Physics.RayCast(targetPosition, direction, out var hit, distance, activePreset.CollisionLayers))
        {
            var bias = activePreset.CollisionDistanceBias + camera.NearPlane;
            var corrected = Mathf.Max(hit.Distance - bias, activePreset.MinDistance);
            camera.Position = targetPosition + direction * corrected;
        }
    }
}
