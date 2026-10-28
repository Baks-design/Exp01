using FlaxEngine;
using Game.Systems.Inputs;

namespace Game.Behaviours.Player;

public static class MovementMath
{
    public static Vector3 Accelerate(Vector3 accelDir, Vector3 prevVelocity, float accelerate,
        float maxVelocity, float dt)
    {
        var projVel = Vector3.Dot(prevVelocity, accelDir);
        var accelVel = accelerate * dt;
        if (projVel + accelVel > maxVelocity) accelVel = maxVelocity - projVel;
        return prevVelocity + accelDir * accelVel;
    }

    public static Vector3 MoveGround(Vector3 accelDir, Vector3 prevVelocity, float friction,
        float groundAccelerate, float maxVelocityGround, float dt)
    {
        var speed = prevVelocity.Length;
        if (speed > 0.01f)
        {
            var drop = speed * friction * dt;
            prevVelocity *= Mathf.Max(speed - drop, 0f) / speed;
        }
        return Accelerate(accelDir, prevVelocity, groundAccelerate, maxVelocityGround, dt);
    }

    public static Vector3 MoveAir(Vector3 accelDir, Vector3 prevVelocity, float airAccelerate,
        float maxVelocityAir, float dt)
        => Accelerate(accelDir, prevVelocity, airAccelerate, maxVelocityAir, dt);

    public static Vector3 ComputeMoveDirection(InputService input, Actor cameraYaw)
    {
        var raw = new Vector3(input.GetAxis(GameAxis.MoveX), 0f, input.GetAxis(GameAxis.MoveY));
        if (raw.LengthSquared > 1f) raw.Normalize();

        var dir = Vector3.Transform(raw, cameraYaw.Orientation);
        dir.Y = 0f;
        if (dir.LengthSquared > 1e-6f)
            dir.Normalize();
        else
            dir = Vector3.Zero;
        return dir;
    }

    public static bool IsGrounded(CharacterController controller, float groundCheck, LayersMask groundLayers)
    {
        if (controller.IsGrounded) return true;

        const float startOffset = 1f;
        var origin = controller.Position - Vector3.Up * (controller.Height * 0.5f - startOffset);

        var length = startOffset + groundCheck;
        return Physics.RayCast(origin, -Vector3.Up, out _, length, groundLayers, false);
    }
}