using FlaxEngine;

namespace Game.Behaviours.Player;

public sealed class PlayerMovementData
{
    public Vector3 verticalVelocity;
    public Vector3 horizontalVelocity;
    public float coyoteTimer;
    public float jumpBufferTimer;
}
