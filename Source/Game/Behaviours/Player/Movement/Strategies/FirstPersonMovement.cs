using FlaxEngine;
using Game.Systems.Inputs;

namespace Game.Behaviours.Player;

public sealed class FirstPersonMovement : IMovementStrategy
{
    FirstPersonMovementPreset preset;

    public JsonAssetReference<FirstPersonMovementPreset> PresetReference { get; set; }

    public void SetPreset(FirstPersonMovementPreset preset) => this.preset = preset;

    public void Execute(MovementContext ctx)
    {
        if (preset == null && PresetReference != null)
            preset = PresetReference.Instance;
        if (preset == null) return;

        var grounded = IsGrounded(ctx);
        var dt = Time.DeltaTime;
        ApplyMovement(ctx, grounded, dt);
        ApplyRotation(ctx, dt);
    }

    void ApplyMovement(MovementContext ctx, bool grounded, float dt)
    {
        if (ctx.movementData.framesSinceJump < int.MaxValue) ctx.movementData.framesSinceJump++;

        var jumpHeld = ctx.input.GetAction(InputActions.Jump);

        UpdateJumpTimers(ctx, jumpHeld, grounded, dt);
        UpdateHorizontalVelocity(ctx, grounded, dt);
        UpdateVerticalVelocity(ctx, jumpHeld, grounded, dt);
        UpdateMove(ctx, dt);
        UpdateFootsteps(ctx, grounded, dt);
    }

    void ApplyRotation(MovementContext ctx, float dt)
    {
        var targetRot = Quaternion.Euler(0, ctx.camera.LocalEulerAngles.Y, 0);
        var t = 1 - Mathf.Exp(-preset.rotationLerpSpeed * dt);
        ctx.controller.Orientation = Quaternion.Lerp(ctx.controller.Orientation, targetRot, t);
    }

    bool IsGrounded(MovementContext ctx)
    {
        if (ctx.controller.IsGrounded) return true;

        var bottom = ctx.controller.Position - Vector3.Up * (ctx.controller.Height * 0.5f - ctx.controller.Radius);
        return Physics.RayCast(bottom, -Vector3.Up, out _, preset.groundCheck, preset.groundLayers, false);
    }

    void UpdateJumpTimers(MovementContext ctx, bool jumpHeld, bool grounded, float dt)
    {
        if (jumpHeld && !ctx.movementData.jumpWasHeld)
        {
            ctx.movementData.jumpBufferTimer = preset.jumpBufferTime;
            ctx.movementData.jumpRequested = true;
        }
        else
            ctx.movementData.jumpBufferTimer = Mathf.Max(ctx.movementData.jumpBufferTimer - dt, 0);

        ctx.movementData.jumpWasHeld = jumpHeld;
        ctx.movementData.coyoteTimer = grounded ? preset.coyoteTime : Mathf.Max(ctx.movementData.coyoteTimer - dt, 0);
    }

    static Vector3 MoveGround(Vector3 accelDir, Vector3 prevVelocity, FirstPersonMovementPreset preset, float dt)
    {
        var speed = prevVelocity.Length;
        if (speed > 0.01f)
        {
            var drop = speed * preset.friction * dt;
            prevVelocity *= Mathf.Max(speed - drop, 0) / speed;
        }
        return Accelerate(accelDir, prevVelocity, preset.groundAccelerate, preset.maxVelocityGround, dt);
    }

    static Vector3 MoveAir(Vector3 accelDir, Vector3 prevVelocity, FirstPersonMovementPreset preset, float dt)
        => Accelerate(accelDir, prevVelocity, preset.airAccelerate, preset.maxVelocityAir, dt);

    static Vector3 Accelerate(Vector3 accelDir, Vector3 prevVelocity, float accelerate, float maxVelocity, float dt)
    {
        var projVel = Vector3.Dot(prevVelocity, accelDir);
        var accelVel = accelerate * dt;
        if (projVel + accelVel > maxVelocity) accelVel = maxVelocity - projVel;
        return prevVelocity + accelDir * accelVel;
    }

    void UpdateHorizontalVelocity(MovementContext ctx, bool grounded, float dt) //FIXME
    {
        var raw = new Vector3(ctx.input.GetAxis(GameAxis.MoveX), 0, ctx.input.GetAxis(GameAxis.MoveY));
        if (raw.LengthSquared > 1) raw.Normalize();

        var yawRot = Quaternion.Euler(0, ctx.camera.Forward.Z, 0);

        var dir = Vector3.Transform(raw, yawRot);
        dir.Y = 0;
        if (dir.LengthSquared > 1) dir.Normalize();

        var horizontal = new Vector3(ctx.movementData.prevVelocity.X, 0, ctx.movementData.prevVelocity.Z);
        horizontal = grounded ? MoveGround(dir, horizontal, preset, dt) : MoveAir(dir, horizontal, preset, dt);
        if (dir.LengthSquared < 1e-6f && horizontal.Length < 0.05f) horizontal = Vector3.Zero;

        ctx.movementData.horizontalVelocity = horizontal;
    }

    void UpdateVerticalVelocity(MovementContext ctx, bool jumpHeld, bool grounded, float dt)
    {
        var v = ctx.movementData.verticalVelocity;
        var gravity = Mathf.Abs(Physics.Gravity.Y) * preset.gravityMultiplier;

        if (grounded)
        {
            if (v.Y < 0) v.Y = 0;

            var wantJump = ctx.movementData.jumpRequested
                        && ctx.movementData.jumpBufferTimer > 0
                        && ctx.movementData.coyoteTimer > 0;
            if (wantJump)
            {
                v.Y = preset.jumpForce;

                ctx.movementData.jumpBufferTimer = 0f;
                ctx.movementData.coyoteTimer = 0f;
                ctx.movementData.jumpRequested = false;
                ctx.movementData.framesSinceJump = 0;

                ctx.sound.PlayJump();
            }
            else if (ctx.movementData.framesSinceJump > preset.jumpGraceFrames)
                v.Y = -Mathf.Abs(Physics.Gravity.Y) * 0.5f;

            if (!wantJump
                && ctx.movementData.lastVerticalVelocity < -1
                && v.Y >= -Mathf.Abs(Physics.Gravity.Y * 0.5f))
                ctx.sound.PlayLand();
        }
        else
        {
            v.Y = ctx.movementData.prevVelocity.Y;
            v.Y -= gravity * dt;
        }

        if ((ctx.controller.Flags & CharacterController.CollisionFlags.Above) != 0 && v.Y > 0) v.Y = 0;

        ctx.movementData.lastVerticalVelocity = v.Y;

        if (!jumpHeld && ctx.movementData.jumpBufferTimer <= 0)
            ctx.movementData.jumpRequested = false;

        ctx.movementData.verticalVelocity = v;
    }

    static void UpdateMove(MovementContext ctx, float dt)
    {
        var finalMove = ctx.movementData.horizontalVelocity + ctx.movementData.verticalVelocity;
        ctx.controller.Move(finalMove * dt);
        ctx.movementData.prevVelocity = finalMove;
    }

    void UpdateFootsteps(MovementContext ctx, bool grounded, float dt)
    {
        var speed = ctx.movementData.horizontalVelocity.Length;
        if (grounded && speed > 1)
        {
            ctx.movementData.footstepDistance += speed * dt;
            if (ctx.movementData.footstepDistance >= preset.footstepDistance)
            {
                ctx.movementData.footstepDistance -= preset.footstepDistance;
                ctx.sound.PlayFootstep();
            }
        }
        else
            ctx.movementData.footstepDistance = 0;
    }
}