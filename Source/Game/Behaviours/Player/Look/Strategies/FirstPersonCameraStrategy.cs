using FlaxEngine;
using Game.Systems.Input;

namespace Game.Behaviours.Player;

public sealed class FirstPersonCameraStrategy : ICameraStrategy
{
    public JsonAssetReference<CameraPreset> PresetReference { get; set; }

    public void Update(
        InputService inputService, Camera camera, Actor targetActor,
        CameraPreset activePreset, ref Vector2 mouseAngles)
    {
        mouseAngles += inputService.GetDelta * activePreset.RotationSpeed;
        mouseAngles.Y = Mathf.Clamp(mouseAngles.Y, -89f, 89f);

        var eyePosition = targetActor.Position + Vector3.Up * activePreset.FirstPersonHeightOffset;
        camera.Position = eyePosition;
        camera.Orientation = Quaternion.Euler(mouseAngles.Y, mouseAngles.X, 0f);
    }
}
