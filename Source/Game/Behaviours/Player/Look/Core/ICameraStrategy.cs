using FlaxEngine;
using Game.Systems.Input;

namespace Game.Behaviours.Player;

public interface ICameraStrategy
{
    void ApplyLook(InputService input, Actor cameraTarget, CameraData data,
        object preset, Camera camera, PlayerCamera self);

    void TriggerShake(CameraData data, float intensity, float duration);

    void AddMovementRotation(CameraData data, float pitch, float yaw);
}

public interface ICameraStrategy<TPreset> : ICameraStrategy where TPreset : class
{
    JsonAssetReference<TPreset> PresetReference { get; }

    void ApplyLook(InputService input, Actor cameraTarget, CameraData data,
        TPreset preset, Camera camera, PlayerCamera self);

    void ICameraStrategy.ApplyLook(InputService input, Actor cameraTarget, CameraData data,
        object preset, Camera camera, PlayerCamera self)
        => ApplyLook(input, cameraTarget, data, (TPreset)preset, camera, self);
}