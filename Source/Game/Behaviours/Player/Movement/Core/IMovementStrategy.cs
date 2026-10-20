using FlaxEngine;
using Game.Systems.Input;

namespace Game.Behaviours.Player;

public interface IMovementStrategy
{
    JsonAssetReference<PlayerMovementPreset> PresetReference { get; }

    MovementResult CalculateMovement(
        InputService inputService,
        PlayerMovementPreset ActivePreset,
        CharacterController Controller,
        PlayerMovementData movementData,
        Camera CameraActor);
}
