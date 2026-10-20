using FlaxEngine;
using Game.Systems.Input;

namespace Game.Behaviours.Player;

public class ThirdPersonMovement : IMovementStrategy
{
    public JsonAssetReference<PlayerMovementPreset> PresetReference { get; set; }

    public MovementResult CalculateMovement(
        InputService inputService,
        CharacterController controller,
        Actor camera,
        PlayerMovementPreset activePreset,
        PlayerMovementData movementData)
    {
        var h = inputService.GetAxis(GameAxis.MoveX);
        var v = inputService.GetAxis(GameAxis.MoveY);

        var camForward = camera.Transform.Forward;
        camForward.Y = 0f;
        camForward.Normalize();
        var camRight = camera.Transform.Right;
        camRight.Y = 0f;
        camRight.Normalize();

        var direction = camForward * v + camRight * h;

        var inputMag = Mathf.Clamp(direction.Length, 0f, 1f);
        if (direction.LengthSquared > 0.0001f) direction.Normalize();

        var targetVelocity = direction * activePreset.Speed * inputMag;

        CharacterMoveState state;
        if (!controller.IsGrounded)
            state = movementData.verticalVelocity.Y > 0f ? CharacterMoveState.Jumping : CharacterMoveState.Falling;
        else
            state = inputMag > 0.1f ? CharacterMoveState.Moving : CharacterMoveState.Idle;

        return new MovementResult
        {
            TargetVelocity = targetVelocity,
            State = state,
            SpeedRatio = inputMag
        };
    }
}