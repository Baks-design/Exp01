using FlaxEngine;
using Game.Systems.Input;

namespace Game.Behaviours.Player.Look;

public sealed class LookSource(AxisPair axes, InputManager injected = null) : ILookSource
{
    readonly InputManager input = injected ?? new();
    readonly AxisPair axes = axes;
    readonly bool ownsInput = injected is null;

    public Float2 Read() => new(input.GetAxis(axes.X), input.GetAxis(axes.Y));

    public void Dispose() { if (ownsInput) input.Dispose(); }
}

