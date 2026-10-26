using FlaxEngine;

namespace Game.Core.Helpers.Timer;

public sealed class TimeService
{
    /// <summary>
    /// Gets the current time scale value from the engine.
    /// </summary>
    public float CurrentTimeScale => Time.TimeScale;

    /// <summary>
    /// Pushes a new time scale to the engine.
    /// </summary>
    /// <param name="scale">The scale factor (1 is normal speed, 0 is paused).</param>
    public void PushTimeScale(float scale) => Time.TimeScale = scale;

    /// <summary>
    /// Resets the time scale back to the default value of 1.
    /// </summary>
    public void ResetTimeScale() => Time.TimeScale = 1;

    /// <summary>
    /// Pauses the game logic by setting the time scale to 0.
    /// </summary>
    public void Pause() => Time.TimeScale = 0;

    /// <summary>
    /// Resumes the game logic by resetting the time scale to 1.
    /// </summary>
    public void Resume() => Time.TimeScale = 1;
}