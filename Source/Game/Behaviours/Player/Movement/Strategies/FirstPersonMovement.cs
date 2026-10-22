using FlaxEngine;
using Game.Systems.Input;

namespace Game.Behaviours.Player;

public sealed class FirstPersonMovement : IMovementStrategy //FIXME:  Movement
{
    public JsonAssetReference<PlayerMovementPreset> PresetReference { get; set; }

    public void ApplyMovement(
        InputService input, CharacterController controller, Actor camera,
        PlayerMovementPreset preset, PlayerMovementData data, CameraData cameraData, PlayerSound sound)
    {
        if (data.framesSinceJump < int.MaxValue) data.framesSinceJump++;

        UpdateJumpTimers(input, controller, preset, data);
        UpdateHorizontalVelocity(input, controller, preset, cameraData, data);
        UpdateVerticalVelocity(controller, preset, data, sound);
        UpdateMove(controller, preset, data, sound);
    }

    public void ApplyRotation(
        CharacterController controller, PlayerMovementPreset preset, PlayerMovementData data,
        Actor characterObj, CameraData cameraData)
    {
        if (data.horizontalVelocity.LengthSquared <= 0.01f) return;

        var targetRot = Quaternion.LookRotation(data.horizontalVelocity.Normalized, Vector3.Up);
        var t = 1 - Mathf.Exp(-preset.FPRotationLerpSpeed * Time.DeltaTime);
        controller.Orientation = Quaternion.Lerp(controller.Transform.Orientation, targetRot, t);
    }

    public void ApplyCameraSmoothing(Camera camera, Actor cameraTarget, PlayerMovementPreset preset)
    {
        var camTrans = camera.Transform;
        var camFactor = 1 - Mathf.Exp(-preset.FPCameraSmoothing * Time.DeltaTime);

        cameraTarget.LocalOrientation = Quaternion.Lerp(
            cameraTarget.LocalOrientation,
            Quaternion.Euler(cameraTarget.LocalEulerAngles.X, cameraTarget.LocalEulerAngles.Y, 0),
            camFactor);

        camTrans.Translation = Vector3.Lerp(camTrans.Translation, cameraTarget.Position, camFactor);
        camTrans.Orientation = cameraTarget.Orientation;
        camera.Transform = camTrans;
    }

    static bool IsGrounded(CharacterController controller, PlayerMovementPreset preset)
    {
        if (controller.IsGrounded) return true;

        return Physics.RayCast(
            controller.Position, -Vector3.Up, out _,
            preset.TPGroundCheck, preset.FPGroundLayers, false);
    }

    static void UpdateJumpTimers(
       InputService input, CharacterController controller, PlayerMovementPreset preset,
       PlayerMovementData data)
    {
        var grounded = IsGrounded(controller, preset);

        data.jumpBufferTimer = input.GetAction(InputActions.Jump)
            ? preset.FPJumpBufferTime
            : data.jumpBufferTimer - Time.DeltaTime;

        data.coyoteTimer = grounded
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
       InputService input, CharacterController controller, PlayerMovementPreset preset,
       CameraData cameraData, PlayerMovementData data)
    {
        var raw = new Vector3(input.GetAxis(GameAxis.MoveX), 0, input.GetAxis(GameAxis.MoveY));
        if (raw.LengthSquared > 1f) raw.Normalize();

        var yawRot = Quaternion.Euler(0, cameraData.yaw, 0);
        var dir = Vector3.Transform(raw, yawRot);
        dir.Y = 0;
        if (dir.LengthSquared > 0f) dir.Normalize();

        var horizontal = new Vector3(data.prevVelocity.X, 0, data.prevVelocity.Z);
        var grounded = IsGrounded(controller, preset);
        horizontal = grounded ? MoveGround(dir, horizontal, preset) : MoveAir(dir, horizontal, preset);
        if (horizontal.Length < 0.05f) horizontal = Vector3.Zero;

        data.horizontalVelocity = horizontal;
    }

    static void UpdateVerticalVelocity(
       CharacterController controller, PlayerMovementPreset preset, PlayerMovementData data, PlayerSound sound)
    {
        var v = data.verticalVelocity;
        var grounded = IsGrounded(controller, preset);
        if (grounded)
        {
            if (v.Y < 0) v.Y = 0;

            var wantJump = data.jumpRequested && data.jumpBufferTimer > 0 && data.coyoteTimer > 0;
            if (wantJump)
            {
                v.Y = preset.FPJumpForce;
                data.jumpBufferTimer = 0;
                data.coyoteTimer = 0;
                data.framesSinceJump = 0;
                sound.PlayJump();
            }
            else if (data.framesSinceJump > preset.FPJumpGraceFrames)
                v.Y = -Mathf.Abs(Physics.Gravity.Y * 0.5f);
        }
        else
        {
            v.Y = data.prevVelocity.Y;
            v.Y += -Mathf.Abs(Physics.Gravity.Y * preset.FPGravityMultiplier) * Time.DeltaTime;
        }

        if (grounded && data.lastVerticalVelocity < -1f && v.Y >= -Mathf.Abs(Physics.Gravity.Y * 0.5f))
            sound.PlayLand();

        data.lastVerticalVelocity = v.Y;
        data.jumpRequested = false;

        if ((controller.Flags & CharacterController.CollisionFlags.Above) != 0 && v.Y > 0) v.Y = 0;

        data.verticalVelocity = v;
    }

    static void UpdateMove(
        CharacterController controller, PlayerMovementPreset preset, PlayerMovementData data, PlayerSound sound)
    {
        var finalMove = data.horizontalVelocity + data.verticalVelocity;
        controller.Move(finalMove * Time.DeltaTime);
        data.prevVelocity = finalMove;

        var speed = data.horizontalVelocity.Length;
        if (IsGrounded(controller, preset) && speed > 1)
        {
            data.footstepDistance += speed * Time.DeltaTime;
            if (data.footstepDistance >= preset.FPFootstepDistance)
            {
                data.footstepDistance -= preset.FPFootstepDistance;
                sound.PlayFootstep();
            }
        }
        else
            data.footstepDistance = 0;
    }

    public void AddMovement(PlayerMovementData data, float horizontal, float vertical)
    {
        data.horizontalVelocity += horizontal;
        data.velocity += vertical;
    }
}