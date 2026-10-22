using FlaxEngine;

namespace Game.Behaviours.Player;

public sealed class PlayerMovementData
{
    public Vector3 horizontalVelocity;
    public Vector3 verticalVelocity;
    public Vector3 prevVelocity;
    public Vector3 velocity;
    public float jumpBufferTimer;
    public float coyoteTimer;
    public bool jumpRequested;
    public bool isGrounded;
    public float footstepDistance;
    public float lastVerticalVelocity;
    public int framesSinceJump = int.MaxValue;

    public void Reset()
    {
        horizontalVelocity = Vector3.Zero;
        verticalVelocity = Vector3.Zero;
        prevVelocity = Vector3.Zero;
        velocity = Vector3.Zero;
        jumpBufferTimer = 0f;
        coyoteTimer = 0;
        footstepDistance = 0;
        lastVerticalVelocity = 0;
    }
}
