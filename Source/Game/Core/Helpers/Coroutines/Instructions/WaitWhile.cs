using System;

namespace Game.Core.Helpers.Coroutines;

/// <summary>
/// Represents a yield instruction to wait while a condition is met.
/// </summary>
public readonly struct WaitWhile(Func<bool> predicate) : IYieldInstruction
{
    readonly Func<bool> predicate = predicate;

    /// <inheritdoc />
    public bool Step(float deltaTime) => !predicate();
}