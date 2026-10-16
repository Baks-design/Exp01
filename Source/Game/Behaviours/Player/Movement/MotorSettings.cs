namespace Game.Behaviours.Player.Movement;

public readonly record struct MotorSettings
{
    public required float MaxSpeed { get; init; }
    public required float SpeedMultiplier { get; init; }
    public required float MoveResponseRate { get; init; }
    public required float AirResponseRate { get; init; }
    public required float JumpMultiplier { get; init; }
    public required float FallGravityMultiplier { get; init; }
    public required float TerminalVelocity { get; init; }
    public required float GroundStickSpeed { get; init; }
    public required float CoyoteTime { get; init; }
    public required float JumpBufferTime { get; init; }
    public required float JumpCooldown { get; init; }
    public required int MaxAirJumps { get; init; }
}
