using System;

namespace Game.Core.Helpers.Coroutines;

/// <summary>
/// Represents a yield instruction to wait until a condition is met.
/// </summary>
public readonly struct WaitUntil(Func<bool> condition) : IYieldInstruction
{
    readonly Func<bool> condition = condition;

    /// <inheritdoc />
    public readonly bool Step(float deltaTime) => condition();
}