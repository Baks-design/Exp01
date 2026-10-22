using FlaxEngine;
using Game.Systems.Input;

namespace Game.Behaviours.Player;

public interface ICameraStrategy
{
    public JsonAssetReference<CameraPreset> PresetReference { get; set; }

    void ApplyLook(
        InputService input,
        Actor cameraTarget,
        CameraData data,
        CameraPreset preset,
        Camera camera);
}
