using System;

namespace Game.Core.Helpers.Coroutines;

public readonly struct WaitWhile(Func<bool> predicate) : IYieldInstruction
{
    readonly Func<bool> predicate = predicate;

    public bool Step(float deltaTime) => !predicate();
}