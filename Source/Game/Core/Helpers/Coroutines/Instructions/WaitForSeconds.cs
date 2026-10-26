using FlaxEngine;

namespace Game.Core.Helpers.Coroutines;

/// <summary>
/// A class that represents a yield instruction to wait for a specific number of seconds.
/// </summary>
public struct WaitForSeconds(float seconds, bool timeScaleIndependent = false) : IYieldInstruction
{
    float secondsRemaining = seconds;
    readonly bool timeScaleIndependent = timeScaleIndependent;

    /// <inheritdoc />
    public bool Step(float deltaTime)
    {
        secondsRemaining -= timeScaleIndependent ? Time.UnscaledDeltaTime : deltaTime;
        return secondsRemaining <= 0;
    }
}