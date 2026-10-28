using FlaxEngine;
using Game.Systems.Inputs;

namespace Game.Behaviours.Player;

public sealed class TopDownMovement : IMovementStrategy
{
    TopDownMovementPreset preset; 

    public JsonAssetReference<TopDownMovementPreset> PresetReference { get; set; }

    public void SetPreset(TopDownMovementPreset preset) => this.preset = preset;

    public void Execute(MovementContext ctx, float dt)
    {
        if (preset == null && PresetReference != null) preset = PresetReference.Instance;
        if (preset == null)
        {
            Debug.LogError("TopDownMovement: preset missing.");
            return;
        }

        ApplyMovement(ctx, dt);
        ApplyRotation(ctx, dt);
    }

    void ApplyMovement(MovementContext ctx, float dt)
    {
        if (ctx.movementData.framesSinceJump < int.MaxValue) ctx.movementData.framesSinceJump++;

        UpdateJumpTimers(ctx, dt);
        UpdateHorizontalVelocity(ctx, dt);
        UpdateVerticalVelocity(ctx, dt);
        UpdateMove(ctx, dt);
        UpdateFootsteps(ctx, dt);
    }

    void ApplyRotation(MovementContext ctx, float dt)
    {
        Vector3 faceDir;
        if (preset.strafe)
            faceDir = Vector3.Transform(Vector3.Forward, Quaternion.Euler(0f, ctx.cameraYaw.Orientation.Y, 0f));
        else
        {
            if (ctx.movementData.horizontalVelocity.LengthSquared <= 0.01f) return;
            faceDir = ctx.movementData.horizontalVelocity.Normalized;
        }

        var currentRot = ctx.controller.Orientation;
        var targetRot = Quaternion.LookRotation(faceDir, Vector3.Up);
        var t = 1f - Mathf.Exp(-preset.rotationLerpSpeed * dt);
        ctx.controller.Orientation = Quaternion.Lerp(currentRot, targetRot, t);
    }

    void UpdateJumpTimers(MovementContext ctx, float dt)
    {
        var jumpHeld = ctx.input.GetAction(InputActions.Jump);
        var grounded = MovementMath.IsGrounded(ctx.controller, preset.groundCheck, preset.groundLayers);

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

    void UpdateHorizontalVelocity(MovementContext ctx, float dt)
    {
        var dir = MovementMath.ComputeMoveDirection(ctx.input, ctx.cameraYaw);

        var horizontal = new Vector3(ctx.movementData.prevVelocity.X, 0f, ctx.movementData.prevVelocity.Z);
        horizontal = MovementMath.IsGrounded(ctx.controller, preset.groundCheck, preset.groundLayers)
            ? MovementMath.MoveGround(dir, horizontal, preset.friction, preset.groundAccelerate, preset.maxVelocityGround, dt)
            : MovementMath.MoveAir(dir, horizontal, preset.airAccelerate, preset.maxVelocityAir, dt);

        if (dir.LengthSquared < 1e-6f && horizontal.Length < 0.05f) horizontal = Vector3.Zero;

        ctx.movementData.horizontalVelocity = horizontal;
    }

    void UpdateVerticalVelocity(MovementContext ctx, float dt)
    {
        var v = ctx.movementData.verticalVelocity;
        var grounded = MovementMath.IsGrounded(ctx.controller, preset.groundCheck, preset.groundLayers);

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
            {
                v.Y = -preset.stickyGroundForce;

                if (ctx.movementData.lastVerticalVelocity < -1f) ctx.sound.PlayLand();
            }
        }
        else
        {
            v.Y = ctx.movementData.prevVelocity.Y;
            v.Y -= Mathf.Abs(Physics.Gravity.Y * preset.gravityMultiplier) * dt;
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

    void UpdateFootsteps(MovementContext ctx, float dt)
    {
        if (!preset.strafe && ctx.movementData.horizontalVelocity.LengthSquared <= 0.01f) return;

        var speed = ctx.movementData.horizontalVelocity.Length;
        if (MovementMath.IsGrounded(ctx.controller, preset.groundCheck, preset.groundLayers) && speed > 1f)
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