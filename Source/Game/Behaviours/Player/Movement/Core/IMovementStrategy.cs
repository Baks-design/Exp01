using FlaxEngine;
using Game.Systems.Input;

namespace Game.Behaviours.Player;

public interface IMovementStrategy
{
    void ApplyCameraSmoothing(Camera camera, Actor cameraTarget, object activePreset);

    void ApplyMovement(InputService input, CharacterController controller, Actor camera,
        object activePreset, PlayerMovementData data, CameraData cameraData, PlayerSound sound);

    void ApplyRotation(CharacterController controller, object activePreset, PlayerMovementData data,
        CameraData cameraData);

    void AddMovement(PlayerMovementData data, float horizontal, float vertical);
}

public interface IMovementStrategy<TPreset> : IMovementStrategy where TPreset : class
{
    JsonAssetReference<TPreset> PresetReference { get; }

    void IMovementStrategy.ApplyCameraSmoothing(Camera camera, Actor cameraTarget, object activePreset)
        => ApplyCameraSmoothing(camera, cameraTarget, (TPreset)activePreset);
        
    void ApplyCameraSmoothing(Camera camera, Actor cameraTarget, TPreset activePreset);

    void IMovementStrategy.ApplyMovement(InputService input, CharacterController controller, Actor camera,
        object activePreset, PlayerMovementData data, CameraData cameraData, PlayerSound sound)
        => ApplyMovement(input, controller, camera, (TPreset)activePreset, data, cameraData, sound);

    void ApplyMovement(InputService input, CharacterController controller, Actor camera,
        TPreset activePreset, PlayerMovementData data, CameraData cameraData, PlayerSound sound);

    void IMovementStrategy.ApplyRotation(CharacterController controller, object activePreset,
        PlayerMovementData data, CameraData cameraData)
        => ApplyRotation(controller, (TPreset)activePreset, data, cameraData);

    void ApplyRotation(CharacterController controller, TPreset activePreset, PlayerMovementData data,
        CameraData cameraData);
}