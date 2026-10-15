using Game.Systems.Input;

namespace Game.Behaviours.Player.Look;

public readonly record struct AxisPair(GameAxis X, GameAxis Y)
{
    public static readonly AxisPair Mouse = new(GameAxis.LookX, GameAxis.LookY);
}

