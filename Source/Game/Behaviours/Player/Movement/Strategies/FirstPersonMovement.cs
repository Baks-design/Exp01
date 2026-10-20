using FlaxEngine;
using Game.Systems.Input;

namespace Game.Behaviours.Player;

public sealed class FirstPersonMovement : IMovementStrategy
{
    public JsonAssetReference<PlayerMovementPreset> PresetReference { get; set; }

    public MovementResult CalculateMovement(
        InputService inputService,
        PlayerMovementPreset ActivePreset,
        CharacterController controller,
        PlayerMovementData movementData,
        Camera CameraActor)
    {
        var p = PresetReference ? PresetReference.Instance : null;
        if (p == null)
        {
            Debug.LogError("ThirdPersonMovement preset is missing!");
            return default;
        }

        var forward = controller.Transform.Forward;
        forward.Y = 0f;
        forward.Normalize();
        var right = controller.Transform.Right;
        right.Y = 0f;
        right.Normalize();

        var direction = forward * inputService.GetAxis(GameAxis.MoveY) + right * inputService.GetAxis(GameAxis.MoveX);
        var inputMag = Mathf.Saturate(direction.Length);
        if (direction.LengthSquared > 0.01f) direction.Normalize();

        var displacement = direction * p.Speed * Time.DeltaTime * inputMag;

        var state = !controller.IsGrounded
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
