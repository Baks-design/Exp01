using FlaxEngine;
using Game.Systems.Inputs;

namespace Game.Behaviours.Player;

public sealed class FirstPersonMovement : IMovementStrategy
{
    FirstPersonMovementPreset preset;

    public JsonAssetReference<FirstPersonMovementPreset> PresetReference { get; set; }

    public void SetPreset(FirstPersonMovementPreset preset) => this.preset = preset;

    public void Execute(MovementContext ctx, float dt)
    {
        if (preset == null && PresetReference != null)
            preset = PresetReference.Instance;
        if (preset == null)
        {
            Debug.LogError("FirstPersonMovement: preset missing.");
            return;
        }

        ApplyMovement(ctx, dt);
        ApplyRotation(ctx, dt);
    }

    void ApplyMovement(MovementContext ctx, float dt)
    {
        if (ctx.movementData.framesSinceJump < int.MaxValue) 
            ctx.movementData.framesSinceJump++;

        var grounded = MovementMath.IsGrounded(ctx.controller, preset.groundCheck, preset.groundLayers);

        UpdateJumpTimers(ctx, grounded, dt);
        UpdateHorizontalVelocity(ctx, grounded, dt);
        UpdateVerticalVelocity(ctx, grounded, dt);
        UpdateMove(ctx, dt);
        UpdateFootsteps(ctx, grounded, dt);
    }

    void ApplyRotation(MovementContext ctx, float dt)
    {
        var currentRot = ctx.controller.Orientation;
        var targetRot = Quaternion.Euler(0f, ctx.cameraYaw.Orientation.Y, 0f);
        var t = 1f - Mathf.Exp(-preset.rotationLerpSpeed * dt);
        ctx.controller.Orientation = Quaternion.Lerp(currentRot, targetRot, t);
    }

    void UpdateJumpTimers(MovementContext ctx, bool grounded, float dt)
    {
        var jumpHeld = ctx.input.GetAction(InputActions.Jump);

        if (jumpHeld && !ctx.movementData.jumpWasHeld)
        {
            ctx.movementData.jumpBufferTimer = preset.jumpBufferTime;
            ctx.movementData.jumpRequested = true;
        }
        else
            ctx.movementData.jumpBufferTimer = Mathf.Max(ctx.movementData.jumpBufferTimer - dt, 0f);

        ctx.movementData.jumpWasHeld = jumpHeld;
        ctx.movementData.coyoteTimer = grounded ? preset.coyoteTime : Mathf.Max(ctx.movementData.coyoteTimer - dt, 0f);
    }

    void UpdateHorizontalVelocity(MovementContext ctx, bool grounded, float dt)
    {
        var dir = MovementMath.ComputeMoveDirection(ctx.input, ctx.cameraYaw);

        var horizontal = new Vector3(ctx.movementData.prevVelocity.X, 0f, ctx.movementData.prevVelocity.Z);
        horizontal = grounded
            ? MovementMath.MoveGround(dir, horizontal, preset.friction, preset.groundAccelerate, preset.maxVelocityGround, dt)
            : MovementMath.MoveAir(dir, horizontal, preset.airAccelerate, preset.maxVelocityAir, dt);

        if (dir.LengthSquared < 1e-6f && horizontal.Length < 0.05f) horizontal = Vector3.Zero;

        ctx.movementData.horizontalVelocity = horizontal;
    }

    void UpdateVerticalVelocity(MovementContext ctx, bool grounded, float dt)
    {
        var v = ctx.movementData.verticalVelocity;
        var gravity = Mathf.Abs(Physics.Gravity.Y) * preset.gravityMultiplier;

        if (grounded)
        {
            if (v.Y < 0f) v.Y = 0f;

            var wantJump = ctx.movementData.jumpRequested
                        && ctx.movementData.jumpBufferTimer > 0f
                        && ctx.movementData.coyoteTimer > 0f;
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
                && ctx.movementData.lastVerticalVelocity < -1f
                && ctx.movementData.framesSinceJump > preset.jumpGraceFrames)
                ctx.sound.PlayLand();
        }
        else
        {
            v.Y = ctx.movementData.prevVelocity.Y;
            v.Y -= gravity * dt;
        }

        if ((ctx.controller.Flags & CharacterController.CollisionFlags.Above) != 0 && v.Y > 0f) v.Y = 0f;

        ctx.movementData.lastVerticalVelocity = v.Y;
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
        if (grounded && speed > 1f)
        {
            ctx.movementData.footstepDistance += speed * dt;
            if (ctx.movementData.footstepDistance >= preset.footstepDistance)
            {
                ctx.movementData.footstepDistance -= preset.footstepDistance;
                ctx.sound.PlayFootstep();
            }
        }
        else ctx.movementData.footstepDistance = 0f;
    }
}