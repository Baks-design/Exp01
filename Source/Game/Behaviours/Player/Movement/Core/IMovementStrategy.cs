using FlaxEngine;
using Game.Systems.Input;

namespace Game.Behaviours.Player;

public interface IMovementStrategy
{
    JsonAssetReference<PlayerMovementPreset> PresetReference { get; }

    MovementResult CalculateMovement(
        InputService inputService,
        CharacterController controller,
        Actor camera,
        PlayerMovementPreset activePreset,
        PlayerMovementData movementData);
}