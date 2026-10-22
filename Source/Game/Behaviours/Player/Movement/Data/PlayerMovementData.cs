using FlaxEngine;

namespace Game.Behaviours.Player;

public sealed class PlayerMovementData
{
    public float jumpBufferTimer;
    public float coyoteTimer;
    public bool jumpRequested;
    public Vector3 horizontalVelocity;
    public Vector3 verticalVelocity;
    public Vector3 prevVelocity;
    public Vector3 Velocity;   
    public bool IsGrounded; 

    public void Reset()
    {
        verticalVelocity = Vector3.Zero;
        horizontalVelocity = Vector3.Zero;
        Velocity = Vector3.Zero;
        coyoteTimer = 0;
        jumpBufferTimer = 0;
    }
}
