using FlaxEngine;
using Game.Systems.Input;

namespace Game.Behaviours.Player;

public sealed class ThirdPersonMovement : IMovementStrategy
{
    public JsonAssetReference<PlayerMovementPreset> PresetReference { get; set; }

    public MovementResult CalculateMovement(
        InputService inputService,
        PlayerMovementPreset ActivePreset,
        CharacterController Controller,
        PlayerMovementData movementData,
        Camera CameraActor)
    {
        var p = PresetReference ? PresetReference.Instance : null;
        if (p == null)
        {
            Debug.LogError("ThirdPersonMovement preset is missing!");
            return default;
        }

        var camF = CameraActor.Transform.Forward;
        camF.Y = 0f;
        camF.Normalize();
        var camR = CameraActor.Transform.Right;
        camR.Y = 0f;
        camR.Normalize();

        var direction = camF * inputService.GetAxis(GameAxis.MoveY) + camR * inputService.GetAxis(GameAxis.MoveX);
        var inputMag = Mathf.Saturate(direction.Length);
        if (direction.LengthSquared > 0.01f) direction.Normalize();

        var displacement = direction * p.Speed * Time.DeltaTime * inputMag;

        var state = !Controller.IsGrounded
            ? movementData.verticalVelocity.Y > 0f ? CharacterMoveState.Jumping : CharacterMoveState.Falling
            : inputMag > 0.1f ? CharacterMoveState.Moving : CharacterMoveState.Idle;

        return new MovementResult
        {
            Displacement = displacement,
            State = state,
            SpeedRatio = inputMag
        };
    }
}
