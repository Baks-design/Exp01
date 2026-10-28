using System.Collections.Generic;
using FlaxEngine;

namespace Game.Core.Helpers.Timer;

public sealed class TimeService
{
    readonly Stack<float> timeScaleStack = new();

    public static float CurrentTimeScale => Time.TimeScale;

    public void PushTimeScale(float scale)
    {
        timeScaleStack.Push(scale);
        ApplyTopScale();
    }

    public void PopTimeScale()
    {
        if (timeScaleStack.Count > 0) timeScaleStack.Pop();

        ApplyTopScale();
    }

    public void ResetTimeScale()
    {
        timeScaleStack.Clear();
        Time.TimeScale = 1f;
    }

    public void Pause() => PushTimeScale(0f);

    public void Resume()
    {
        if (timeScaleStack.Count > 0 && timeScaleStack.Peek() == 0f)
            PopTimeScale();
        else if (timeScaleStack.Count == 0)
            Time.TimeScale = 1f;
    }

    void ApplyTopScale() => Time.TimeScale = timeScaleStack.Count > 0 ? timeScaleStack.Peek() : 1f;
}