using FlaxEngine;
using Game.Systems.Input;

namespace Game.Behaviours.Player.Look;

public sealed class LookSource(AxisPair axes, IInputService injected = null) : ILookSource
{
    readonly IInputService input = injected ?? new InputService();
    readonly AxisPair axes = axes;

    public Float2 Read() => new(input.GetAxis(axes.X), input.GetAxis(axes.Y));
}

