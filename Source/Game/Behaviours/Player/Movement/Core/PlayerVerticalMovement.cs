using FlaxEngine;
using Game.Systems.Input;

namespace Game.Behaviours.Player;

public sealed class PlayerVerticalMovement(
    InputService inputService,
    CharacterController controller,
    PlayerMovementData movementData)
{
    readonly InputService inputService = inputService;
    readonly CharacterController controller = controller;
    readonly PlayerMovementData movementData = movementData;
    float coyoteTimer;
    float jumpBufferTimer;

    public void UpdateJumpTimers(PlayerMovementPreset preset)
    {
        jumpBufferTimer = inputService.GetAction(InputActions.Jump)
            ? preset.JumpBufferTime
            : jumpBufferTimer - Time.DeltaTime;

        coyoteTimer = controller.IsGrounded
            ? preset.CoyoteTime
            : coyoteTimer - Time.DeltaTime;
    }

    public void ApplyVerticalMovement(bool grounded, PlayerMovementPreset preset)
    {
        if (grounded)
        {
            if (movementData.verticalVelocity.Y < 0f)
                movementData.verticalVelocity.Y = 0f;

            if (jumpBufferTimer > 0f && coyoteTimer > 0f)
            {
                movementData.verticalVelocity.Y = preset.JumpForce;
                jumpBufferTimer = 0f;
                coyoteTimer = 0f;
            }
        }
        else
        {
            movementData.verticalVelocity.Y += preset.GravityMultiplier * Time.DeltaTime;
        }
    }
}