namespace Game.Behaviours.Player;

public readonly record struct Parameters(string Name)
{
    public static readonly string Speed = new("Speed");
    public static readonly string IsGrounded = new("IsGrounded");
}
