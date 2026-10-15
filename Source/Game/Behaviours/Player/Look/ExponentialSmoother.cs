using FlaxEngine;

namespace Game.Behaviours.Player.Look;

public sealed class ExponentialSmoother(float decay) : ILookSmoother
{
    public float Step(float current, float target, float deltaTime)
        => Mathf.Lerp(current, target, 1f - Mathf.Exp(-decay * deltaTime));

    public void Reset() { }
}

