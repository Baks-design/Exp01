using FlaxEngine;
using Game.Systems.Input;

namespace Game.Behaviours.Player;

public interface IMovementStrategy
{
    JsonAssetReference<PlayerMovementPreset> PresetReference { get; }

    void ApplyCameraSmoothing(
        Camera camera,
        Actor cameraTarget,
        PlayerMovementPreset preset);
    void ApplyMovement(
        InputService input,
        CharacterController controller,
        Actor camera,
        PlayerMovementPreset preset,
        PlayerMovementData data,
        CameraData cameraData,
        PlayerSound sound);
    void ApplyRotation(
        CharacterController controller,
        PlayerMovementPreset preset,
        PlayerMovementData data,
        Actor characterObj,
        CameraData cameraData);
    void AddMovement(
        PlayerMovementData data,
        float horizontal,
        float vertical);
}