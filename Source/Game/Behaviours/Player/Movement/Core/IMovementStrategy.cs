using FlaxEngine;
using Game.Systems.Input;

namespace Game.Behaviours.Player;

public interface IMovementStrategy
{
    JsonAssetReference<PlayerMovementPreset> PresetReference { get; }

    void ApplyMovement(InputService input,
        CharacterController controller,
        Actor camera,
        PlayerMovementPreset preset,
        PlayerMovementData data);
    void ApplyRotation(
        CharacterController controller,
        PlayerMovementPreset preset,
        PlayerMovementData data);
}