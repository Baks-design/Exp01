using FlaxEngine;
using Game.Core.Timer;

namespace Game.Core.Helpers.Timer;

public sealed class Transition(ITimeService time)
{
    readonly ITimeService time = time;
    float start, target, duration, elapsed;
    bool active;

    public TransitionTimeMode TimeMode { get; set; } = TransitionTimeMode.Scaled;
    public float Value { get; private set; }

    public void Begin(float from, float to, float duration)
    {
        start = from;
        target = to;
        this.duration = duration;
        elapsed = 0f;
        active = true;
    }

    public void Tick()
    {
        if (!active) return;

        var dt = TimeMode switch
        {
            TransitionTimeMode.Scaled => time.DeltaTime,
            TransitionTimeMode.Unscaled => time.UnscaledDeltaTime,
            _ => time.DeltaTime
        };
        if (dt <= 0f) return;
        elapsed += dt;
        var t = 1f - Mathf.Exp(-duration * elapsed);
        Value = Mathf.Lerp(start, target, t);
        if (t >= 1f)
        {
            Value = target;
            active = false;
        }
    }
}