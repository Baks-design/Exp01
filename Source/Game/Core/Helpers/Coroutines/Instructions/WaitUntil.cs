using System;

namespace Game.Core.Helpers.Coroutines;

public readonly struct WaitUntil(Func<bool> condition) : IYieldInstruction
{
    readonly Func<bool> condition = condition;

    public bool Step(float deltaTime) => condition();
}