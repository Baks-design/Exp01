using FlaxEngine;

namespace Game.Behaviours.Player.Movement;

public sealed class CharacterMotor(CharacterController controller, in MotorSettings settings)
{
    readonly CharacterController controller = controller;
    MotorSettings settings = settings;
    Vector3 horizontalVelocity;
    float verticalVelocity;
    int airJumpsRemaining;
    float coyoteTimer;
    float jumpBufferTimer;
    float jumpCooldownTimer;
    bool wasGrounded;

    public bool IsGrounded => controller.IsGrounded;
    public Vector3 Velocity => horizontalVelocity + Vector3.Up * verticalVelocity;
    public Vector3 HorizontalVelocity => horizontalVelocity;

    public void ApplySettings(in MotorSettings settings) => this.settings = settings;

    public void Step(in MovementIntent intent, float dt)
    {
        if (dt <= 0f) return;

        UpdateGroundedState(dt);
        UpdateTimers(dt);
        UpdateJump(intent.JumpPressed);
        UpdateHorizontal(intent, dt);
        UpdateVertical(dt);
        Integrate(dt);
    }

    void UpdateGroundedState(float _)
    {
        var grounded = controller.IsGrounded;

        if (grounded)
        {
            coyoteTimer = settings.CoyoteTime;
            airJumpsRemaining = settings.MaxAirJumps;
            if (verticalVelocity < 0f) verticalVelocity = -settings.GroundStickSpeed;
        }
        else if (wasGrounded) coyoteTimer = settings.CoyoteTime;

        wasGrounded = grounded;
    }

    void UpdateTimers(float dt)
    {
        if (coyoteTimer > 0f) coyoteTimer = Mathf.Max(0f, coyoteTimer - dt);
        if (jumpBufferTimer > 0f) jumpBufferTimer = Mathf.Max(0f, jumpBufferTimer - dt);
        if (jumpCooldownTimer > 0f) jumpCooldownTimer = Mathf.Max(0f, jumpCooldownTimer - dt);
    }

    void UpdateJump(bool pressedThisStep)
    {
        if (pressedThisStep && jumpCooldownTimer <= 0f) jumpBufferTimer = settings.JumpBufferTime;
        if (jumpBufferTimer <= 0f) return;

        var canGroundJump = coyoteTimer > 0f;
        var canAirJump = !canGroundJump && airJumpsRemaining > 0;
        if (!canGroundJump && !canAirJump) return;

        verticalVelocity = Mathf.Sqrt(2f * settings.JumpHeight * settings.Gravity);

        if (canAirJump) airJumpsRemaining--;
        else coyoteTimer = 0f;

        jumpBufferTimer = 0f;
        jumpCooldownTimer = settings.JumpCooldown;
    }

    void UpdateHorizontal(in MovementIntent intent, float dt)
    {
        var maxSpeed = intent.SprintHeld ? settings.MaxSpeed * 1.5f : settings.MaxSpeed;

        var target = intent.Direction * maxSpeed;
        var rate = controller.IsGrounded ? settings.MoveResponseRate : settings.AirResponseRate;

        var t = 1f - Mathf.Exp(-rate * dt);
        horizontalVelocity = Vector3.Lerp(horizontalVelocity, target, t);

        if ((target - horizontalVelocity).LengthSquared < 1e-8f) horizontalVelocity = target;
    }

    void UpdateVertical(float dt)
    {
        if (controller.IsGrounded && verticalVelocity <= 0f) return;

        var g = settings.Gravity;
        if (verticalVelocity < 0f) g *= settings.FallGravityMultiplier;

        verticalVelocity -= g * dt;
        verticalVelocity = Mathf.Max(verticalVelocity, -settings.TerminalVelocity);
    }

    void Integrate(float dt)
    {
        var displacement = horizontalVelocity * dt;
        displacement.Y = verticalVelocity * dt;
        controller.Move(displacement);
    }
}
