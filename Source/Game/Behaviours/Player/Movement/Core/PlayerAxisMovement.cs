using FlaxEngine;
using Game.Systems.Input;

namespace Game.Behaviours.Player;

public sealed class PlayerAxisMovement(
    InputService inputService,
    CharacterController controller,
    PlayerMovementData movementData)
{
    private readonly InputService inputService = inputService;
    private readonly CharacterController controller = controller;
    private readonly PlayerMovementData movementData = movementData;
    private float coyoteTimer;
    private float jumpBufferTimer;

    public void UpdateJumpTimers(PlayerMovementPreset preset)
    {
        jumpBufferTimer = inputService.GetAction(InputActions.Jump)
            ? preset.JumpBufferTime
            : jumpBufferTimer - Time.DeltaTime;
            
        coyoteTimer = controller.IsGrounded
            ? preset.CoyoteTime
            : coyoteTimer - Time.DeltaTime;
    }

    public void UpdateVerticalVelocity(bool grounded, PlayerMovementPreset preset)
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
            movementData.verticalVelocity.Y += preset.GravityMultiplier * Physics.Gravity.Y * Time.DeltaTime;
    }

    public void UpdateHorizontalVelocity(Vector3 targetVelocity, PlayerMovementPreset preset)
    {
        var hasInput = targetVelocity.LengthSquared > 0.01f;
        var lerpRate = hasInput ? preset.Acceleration : preset.Deceleration;

        var t = 1f - Mathf.Exp(-lerpRate * Time.DeltaTime);
        movementData.horizontalVelocity = Vector3.Lerp(movementData.horizontalVelocity, targetVelocity, t);

        if (movementData.horizontalVelocity.LengthSquared < 0.01f && !hasInput)
            movementData.horizontalVelocity = Vector3.Zero;
    }
}