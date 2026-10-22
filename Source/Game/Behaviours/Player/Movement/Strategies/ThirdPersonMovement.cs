using FlaxEngine;
using Game.Systems.Input;

namespace Game.Behaviours.Player;

public sealed class ThirdPersonMovement : IMovementStrategy //TODO: Apply Smooth
{
    public JsonAssetReference<PlayerMovementPreset> PresetReference { get; set; }

    public void ApplyCameraSmoothing(Camera camera, Actor cameraTarget, PlayerMovementPreset preset) { }

    public void ApplyMovement(
        InputService input, CharacterController controller, Actor camera,
        PlayerMovementPreset preset, PlayerMovementData data, CameraData cameraData)
    {
        var movement = new Vector3(input.GetAxis(GameAxis.MoveX), 0, input.GetAxis(GameAxis.MoveY));
        var movementDirection = camera.Transform.TransformDirection(movement);

        if (input.GetAction(InputActions.Jump) && controller.IsGrounded)
            data.Velocity.Y = Mathf.Sqrt(preset.JumpStrength * -2 * Physics.Gravity.Y);

        data.Velocity.Y += preset.GravityMultiplier * Physics.Gravity.Y * Time.DeltaTime;
        movementDirection += data.Velocity * 0.5f;

        var speed = (input.GetAction(InputActions.Sprint) ? preset.SprintSpeed : preset.Speed) * Time.DeltaTime;
        controller.Move(movementDirection * speed);
    }

    public void ApplyRotation(
        CharacterController controller, PlayerMovementPreset preset, PlayerMovementData data,
        Actor characterObj, CameraData cameraData)
    {
        characterObj.Orientation = Quaternion.Euler(0, cameraData.yaw, 0);
    }
}