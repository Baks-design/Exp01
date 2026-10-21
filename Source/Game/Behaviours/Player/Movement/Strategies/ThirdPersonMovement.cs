using FlaxEngine;
using Game.Systems.Input;

namespace Game.Behaviours.Player;

public class ThirdPersonMovement : IMovementStrategy
{
    public JsonAssetReference<PlayerMovementPreset> PresetReference { get; set; }

    static void UpdateJumpTimers(
        InputService input, CharacterController controller, PlayerMovementPreset preset, PlayerMovementData data)
    {
        data.jumpBufferTimer = input.GetAction(InputActions.Jump) ? preset.TPJumpBufferTime : data.jumpBufferTimer - Time.DeltaTime;
        data.coyoteTimer = controller.IsGrounded ? preset.TPCoyoteTime : data.coyoteTimer - Time.DeltaTime;
    }

    static void UpdateMovement(
        InputService input, Actor camera, PlayerMovementPreset preset, PlayerMovementData data)
    {
        var camForward = camera.Transform.Forward;
        camForward.Y = 0;
        camForward.Normalize();
        var camRight = camera.Transform.Right;
        camRight.Y = 0;
        camRight.Normalize();

        var hv = new Float2(input.GetAxis(GameAxis.MoveX), input.GetAxis(GameAxis.MoveY));
        var direction = camForward * hv.Y + camRight * hv.X;

        var inputMag = Mathf.Saturate(direction.Length);
        
        if (direction.LengthSquared > 0.0001f) direction.Normalize();

        var speed = input.GetAction(InputActions.Sprint) ? preset.TPSpeed : preset.TPRunSpeed;

        data.targetVelocity = inputMag * direction * speed;
    }

    static void UpdateHorizontalVelocity(PlayerMovementData data, PlayerMovementPreset preset)
    {
        var hasInput = data.targetVelocity.LengthSquared > 0.01f;
        var lerpRate = hasInput ? preset.TPAcceleration : preset.TPDeceleration;

        var t = 1 - Mathf.Exp(-lerpRate * Time.DeltaTime);
        data.horizontalVelocity = Vector3.Lerp(data.horizontalVelocity, data.targetVelocity, t);
        if (data.horizontalVelocity.LengthSquared < 0.01f && !hasInput) 
            data.horizontalVelocity = Vector3.Zero;
    }

    static void UpdateVerticalVelocity(CharacterController controller, PlayerMovementData data, PlayerMovementPreset preset)
    {
        if (controller.IsGrounded)
        {
            if (data.verticalVelocity.Y < 0) data.verticalVelocity.Y = 0;

            if (data.jumpBufferTimer > 0 && data.coyoteTimer > 0)
            {
                data.verticalVelocity.Y = preset.TPJumpForce;
                data.jumpBufferTimer = 0;
                data.coyoteTimer = 0;
            }
        }
        else
            data.verticalVelocity.Y += preset.TPGravityMultiplier * Physics.Gravity.Y * Time.DeltaTime;
    }

    static void UpdateMove(CharacterController controller, PlayerMovementData data)
    {
        var finalMove = (data.horizontalVelocity + data.verticalVelocity) * Time.DeltaTime;
        controller.Move(finalMove);
    }

    public void ApplyMovement(
        InputService input,
        CharacterController controller,
        Actor camera,
        PlayerMovementPreset preset,
        PlayerMovementData data)
    {
        UpdateJumpTimers(input, controller, preset, data);
        UpdateMovement(input, camera, preset, data);
        UpdateHorizontalVelocity(data, preset);
        UpdateVerticalVelocity(controller, data, preset);
        UpdateMove(controller, data);
    }

    public void ApplyRotation(CharacterController controller, PlayerMovementPreset preset, PlayerMovementData data)
    {
        if (data.horizontalVelocity.LengthSquared <= 0.01f) return;

        var targetRot = Quaternion.LookRotation(data.horizontalVelocity.Normalized, Vector3.Up);
        var t = Time.DeltaTime * preset.TPRotationLerpSpeed;
        controller.Orientation = Quaternion.Lerp(controller.Transform.Orientation, targetRot, t);
    }
}