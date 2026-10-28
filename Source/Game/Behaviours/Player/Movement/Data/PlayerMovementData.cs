using FlaxEngine;

namespace Game.Behaviours.Player;

public sealed class PlayerMovementData
{
    public Vector3 horizontalVelocity;
    public Vector3 verticalVelocity;
    public Vector3 prevVelocity;
    public float jumpBufferTimer;
    public float coyoteTimer;
    public bool jumpRequested;
    public float footstepDistance;
    public float lastVerticalVelocity;
    public int framesSinceJump = int.MaxValue;
    public bool jumpWasHeld;

    public void Reset()
    {
        horizontalVelocity = Vector3.Zero;
        verticalVelocity = Vector3.Zero;
        prevVelocity = Vector3.Zero;
        jumpBufferTimer = 0f;
        coyoteTimer = 0f;
        jumpRequested = false;
        footstepDistance = 0f;
        lastVerticalVelocity = 0f;
        framesSinceJump = int.MaxValue;
        jumpWasHeld = false;
    }
}