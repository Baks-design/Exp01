using FlaxEngine;

namespace Game.Behaviours.Player;

public sealed class PlayerMovementData
{
    public Vector3 verticalVelocity;
    public Vector3 horizontalVelocity;
    public Vector3 targetVelocity;
    public float coyoteTimer;
    public float jumpBufferTimer;

    public void Reset()
    {
        verticalVelocity = Vector3.Zero;
        horizontalVelocity = Vector3.Zero;
        targetVelocity = Vector3.Zero;
        coyoteTimer = 0;
        jumpBufferTimer = 0;
    }
}
