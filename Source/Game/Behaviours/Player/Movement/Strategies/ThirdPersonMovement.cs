using FlaxEngine;
using Game.Systems.Inputs;

namespace Game.Behaviours.Player;

public sealed class ThirdPersonMovement : IMovementStrategy
{
    ThirdPersonMovementPreset preset;

    public JsonAssetReference<ThirdPersonMovementPreset> PresetReference { get; set; }

    public void SetPreset(ThirdPersonMovementPreset preset) => this.preset = preset;

    public void Execute(MovementContext ctx)
    {
        if (preset == null) return;
        
        ApplyCameraSmoothing(ctx);
        ApplyMovement(ctx);
        ApplyRotation(ctx);
    }

    void ApplyCameraSmoothing(MovementContext ctx)
    {
        var camTrans = ctx.camera.Transform;
        var camFactor = 1 - Mathf.Exp(-preset.cameraSmoothing * Time.DeltaTime);

        ctx.camera.LocalOrientation = Quaternion.Lerp(
            ctx.camera.LocalOrientation,
            Quaternion.Euler(ctx.camera.LocalEulerAngles.X, ctx.camera.LocalEulerAngles.Y, 0),
            camFactor);

        camTrans.Translation = Vector3.Lerp(camTrans.Translation, ctx.camera.Position, camFactor);
        camTrans.Orientation = ctx.camera.Orientation;
        ctx.camera.Transform = camTrans;
    }

    void ApplyMovement(MovementContext ctx)
    {
        if (ctx.movementData.framesSinceJump < int.MaxValue)
            ctx.movementData.framesSinceJump++;

        UpdateJumpTimers(ctx);
        UpdateHorizontalVelocity(ctx);
        UpdateVerticalVelocity(ctx);
        UpdateMove(ctx);
    }

    void ApplyRotation(MovementContext ctx)
    {
        Vector3 faceDir;
        if (preset.strafe)
            faceDir = Vector3.Transform(Vector3.Forward, Quaternion.Euler(0, ctx.cameraData.yaw, 0));
        else
        {
            if (ctx.movementData.horizontalVelocity.LengthSquared <= 0.01f) return;
            faceDir = ctx.movementData.horizontalVelocity.Normalized;
        }

        var targetRot = Quaternion.LookRotation(faceDir, Vector3.Up);
        var t = 1 - Mathf.Exp(-preset.rotationLerpSpeed * Time.DeltaTime);
        ctx.controller.Orientation = Quaternion.Lerp(ctx.controller.Transform.Orientation, targetRot, t);
    }

    bool IsGrounded(MovementContext ctx)
    {
        if (ctx.controller.IsGrounded) return true;

        return Physics.RayCast(ctx.controller.Position, -Vector3.Up, out _, preset.groundCheck,
            preset.groundLayers, false);
    }

    void UpdateJumpTimers(MovementContext ctx)
    {
        ctx.movementData.jumpBufferTimer = ctx.input.GetAction(InputActions.Jump)
            ? preset.jumpBufferTime
            : ctx.movementData.jumpBufferTimer - Time.DeltaTime;

        ctx.movementData.coyoteTimer = IsGrounded(ctx)
            ? preset.coyoteTime
            : ctx.movementData.coyoteTimer - Time.DeltaTime;

        if (ctx.input.GetAction(InputActions.Jump))
            ctx.movementData.jumpRequested = true;
    }

    static Vector3 MoveGround(Vector3 accelDir, Vector3 prevVelocity, ThirdPersonMovementPreset preset)
    {
        var speed = prevVelocity.Length;
        if (Mathf.Abs(speed) > 0.01f)
        {
            var drop = speed * preset.friction * Time.DeltaTime;
            prevVelocity *= Mathf.Max(speed - drop, 0) / speed;
        }
        return Accelerate(accelDir, prevVelocity, preset.groundAccelerate, preset.maxVelocityGround);
    }

    static Vector3 MoveAir(Vector3 accelDir, Vector3 prevVelocity, ThirdPersonMovementPreset preset)
      => Accelerate(accelDir, prevVelocity, preset.airAccelerate, preset.maxVelocityAir);

    static Vector3 Accelerate(Vector3 accelDir, Vector3 prevVelocity, float accelerate, float maxVelocity)
    {
        var projVel = Vector3.Dot(prevVelocity, accelDir);
        var accelVel = accelerate * Time.DeltaTime;
        if (projVel + accelVel > maxVelocity) accelVel = maxVelocity - projVel;
        return prevVelocity + accelDir * accelVel;
    }

    void UpdateHorizontalVelocity(MovementContext ctx)
    {
        var raw = new Vector3(ctx.input.GetAxis(GameAxis.MoveX), 0, ctx.input.GetAxis(GameAxis.MoveY));
        if (raw.LengthSquared > 1) raw.Normalize();

        var yawRot = Quaternion.Euler(0, ctx.cameraData.yaw, 0);
        var dir = Vector3.Transform(raw, yawRot);
        dir.Y = 0;
        if (dir.LengthSquared > 0) dir.Normalize();

        var horizontal = new Vector3(ctx.movementData.prevVelocity.X, 0, ctx.movementData.prevVelocity.Z);

        horizontal = IsGrounded(ctx) ? MoveGround(dir, horizontal, preset) : MoveAir(dir, horizontal, preset);
        if (horizontal.Length < 0.05f) horizontal = Vector3.Zero;

        ctx.movementData.horizontalVelocity = horizontal;
    }

    void UpdateVerticalVelocity(MovementContext ctx)
    {
        var v = ctx.movementData.verticalVelocity;

        if (IsGrounded(ctx))
        {
            if (v.Y < 0) v.Y = 0;

            var wantJump = ctx.movementData.jumpRequested &&
                ctx.movementData.jumpBufferTimer > 0 &&
                ctx.movementData.coyoteTimer > 0;
            if (wantJump)
            {
                v.Y = preset.jumpForce;
                ctx.movementData.jumpBufferTimer = 0;
                ctx.movementData.coyoteTimer = 0;
                ctx.movementData.framesSinceJump = 0;
            }
            else if (ctx.movementData.framesSinceJump > preset.jumpGraceFrames)
                v.Y = -preset.stickyGroundForce;
        }
        else
        {
            v.Y = ctx.movementData.prevVelocity.Y;
            v.Y += -Mathf.Abs(Physics.Gravity.Y * preset.gravityMultiplier) * Time.DeltaTime;
        }

        ctx.movementData.jumpRequested = false;

        if ((ctx.controller.Flags & CharacterController.CollisionFlags.Above) != 0 && v.Y > 0) 
            v.Y = 0;

        ctx.movementData.verticalVelocity = v;
    }

    static void UpdateMove(MovementContext ctx)
    {
        var finalMove = ctx.movementData.horizontalVelocity + ctx.movementData.verticalVelocity;
        ctx.controller.Move(finalMove * Time.DeltaTime);
        ctx.movementData.prevVelocity = finalMove;
    }
}