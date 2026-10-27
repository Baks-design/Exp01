using FlaxEngine;

namespace Game.Core.Helpers.Coroutines;

public struct WaitForSeconds(float seconds, bool timeScaleIndependent = false) : IYieldInstruction
{
    float secondsRemaining = seconds;
    readonly bool timeScaleIndependent = timeScaleIndependent;

    public bool Step(float deltaTime)
    {
        secondsRemaining -= timeScaleIndependent ? Time.UnscaledDeltaTime : deltaTime;
        return secondsRemaining <= 0;
    }
}