using FlaxEngine;

namespace Game.Core.Helpers.Coroutines;

public struct WaitForSeconds(float seconds, bool timeScaleIndependent = false) : IYieldInstruction
{
    float remaining = seconds;
    readonly bool unscaled = timeScaleIndependent;

    public bool Step(float deltaTime)
    {
        remaining -= unscaled ? Time.UnscaledDeltaTime : deltaTime;
        return remaining <= 0f;
    }
}
