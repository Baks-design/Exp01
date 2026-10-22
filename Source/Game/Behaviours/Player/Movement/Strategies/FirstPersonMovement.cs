using FlaxEngine;
using Game.Systems.Input;

namespace Game.Behaviours.Player;

public sealed class FirstPersonMovement : IMovementStrategy //TODO: Apply Smooth
{
    public JsonAssetReference<PlayerMovementPreset> PresetReference { get; set; }

    public void ApplyMovement(
        InputService input, CharacterController controller, Actor camera,
        PlayerMovementPreset preset, PlayerMovementData data, CameraData cameraData)
    {
        UpdateJumpTimers(input, controller, preset, data);
        UpdateHorizontalVelocity(input, controller, preset, cameraData, data);
        UpdateVerticalVelocity(controller, preset, data);
        UpdateMove(controller, data);
    }

    public void ApplyRotation(
        CharacterController controller, PlayerMovementPreset preset, PlayerMovementData data,
        Actor characterObj, CameraData cameraData)
    {
        if (data.horizontalVelocity.LengthSquared <= 0.01f) return;

        var targetRot = Quaternion.LookRotation(data.horizontalVelocity.Normalized, Vector3.Up);
        var t = Time.DeltaTime * preset.FPRotationLerpSpeed;
        controller.Orientation = Quaternion.Lerp(controller.Transform.Orientation, targetRot, t);
    }

    public void ApplyCameraSmoothing(Camera camera, Actor cameraTarget, PlayerMovementPreset preset)
    {
        var camTrans = camera.Transform;
        var camFactor = Mathf.Saturate(preset.FPSmoothing * Time.DeltaTime);

        cameraTarget.LocalOrientation = Quaternion.Lerp(
            cameraTarget.LocalOrientation,
            Quaternion.Euler(cameraTarget.LocalEulerAngles.X, cameraTarget.LocalEulerAngles.Y, 0),
            camFactor);

        camTrans.Translation = Vector3.Lerp(camTrans.Translation, cameraTarget.Position, camFactor);
        camTrans.Orientation = cameraTarget.Orientation;
        camera.Transform = camTrans;
    }

    static void UpdateJumpTimers(
        InputService input, CharacterController controller, PlayerMovementPreset preset, PlayerMovementData data)
    {
        data.jumpBufferTimer = input.GetAction(InputActions.Jump)
            ? preset.FPJumpBufferTime
            : data.jumpBufferTimer - Time.DeltaTime;

        data.coyoteTimer = controller.IsGrounded
            ? preset.FPCoyoteTime
            : data.coyoteTimer - Time.DeltaTime;

        if (input.GetAction(InputActions.Jump)) data.jumpRequested = true;
    }

    static Vector3 Accelerate(Vector3 accelDir, Vector3 prevVelocity, float accelerate, float maxVelocity)
    {
        var projVel = (float)Vector3.Dot(prevVelocity, accelDir);
        var accelVel = accelerate * Time.DeltaTime;
        if (projVel + accelVel > maxVelocity) accelVel = maxVelocity - projVel;
        return prevVelocity + accelDir * accelVel;
    }

    static Vector3 MoveGround(Vector3 accelDir, Vector3 prevVelocity, PlayerMovementPreset preset)
    {
        var speed = prevVelocity.Length;
        if (Mathf.Abs(speed) > 0.01f)
        {
            var drop = speed * preset.FPFriction * Time.DeltaTime;
            prevVelocity *= Mathf.Max(speed - drop, 0) / speed;
        }
        return Accelerate(accelDir, prevVelocity, preset.FPGroundAccelerate, preset.FPMaxVelocityGround);
    }

    static Vector3 MoveAir(Vector3 accelDir, Vector3 prevVelocity, PlayerMovementPreset preset)
        => Accelerate(accelDir, prevVelocity, preset.FPAirAccelerate, preset.FPMaxVelocityAir);

    static void UpdateHorizontalVelocity(
        InputService input, CharacterController controller, PlayerMovementPreset movementPreset,
        CameraData cameraData, PlayerMovementData movementData)
    {
        var dir = new Vector3(input.GetAxis(GameAxis.MoveX), 0, input.GetAxis(GameAxis.MoveY));
        dir.Normalize();

        var yawRot = Quaternion.Euler(0, cameraData.yaw, 0);
        dir = Vector3.Transform(dir, yawRot);

        var horizontal = new Vector3(movementData.prevVelocity.X, 0, movementData.prevVelocity.Z);
        horizontal = controller.IsGrounded ?
            MoveGround(dir.Normalized, horizontal, movementPreset) :
            MoveAir(dir.Normalized, horizontal, movementPreset);
        if (horizontal.Length < 0.05f) horizontal = Vector3.Zero;

        movementData.horizontalVelocity = horizontal;
    }

    static void UpdateVerticalVelocity(
        CharacterController controller, PlayerMovementPreset preset, PlayerMovementData data)
    {
        var v = data.verticalVelocity;

        if (controller.IsGrounded)
        {
            if (v.Y < 0) v.Y = 0;

            if (data.jumpRequested && data.jumpBufferTimer > 0 && data.coyoteTimer > 0)
            {
                v.Y = preset.FPJumpForce;
                data.jumpBufferTimer = 0;
                data.coyoteTimer = 0;
            }

            v.Y = -Mathf.Abs(Physics.Gravity.Y * 0.5f);
        }
        else
            v.Y = data.prevVelocity.Y;

        data.jumpRequested = false;

        v.Y += -Mathf.Abs(Physics.Gravity.Y * preset.FPGravityMultiplier) * Time.DeltaTime;
        if ((controller.Flags & CharacterController.CollisionFlags.Above) != 0 && v.Y > 0) v.Y = 0;

        data.verticalVelocity = v;
    }

    static void UpdateMove(CharacterController controller, PlayerMovementData data)
    {
        var finalMove = data.horizontalVelocity + data.verticalVelocity;
        controller.Move(finalMove * Time.DeltaTime);
        data.prevVelocity = finalMove;
    }
}