using FlaxEngine;

namespace Game.Core.Helpers.Timer;

public sealed class TimeService
{
    public float CurrentTimeScale => Time.TimeScale;

    public void PushTimeScale(float scale) => Time.TimeScale = scale;

    public void ResetTimeScale() => Time.TimeScale = 1;

    public void Pause() => Time.TimeScale = 0;

    public void Resume() => Time.TimeScale = 1;
}