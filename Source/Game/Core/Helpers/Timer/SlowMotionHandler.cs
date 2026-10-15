using FlaxEngine;
using Game.Core.Timer;

namespace Game.Core.Helpers.Timer;

public sealed class SlowMotionHandler
{
    Transition fade;
    ITimeService time;

    public void Init()
    {
        time = new TimeService();
        fade = new Transition(time) { TimeMode = TransitionTimeMode.Unscaled };
        fade.Begin(1f, 1f, 0.001f);
        Time.TimeScale = 1f;
    }

    public void Update()
    {
        fade.Tick();
        Time.TimeScale = fade.Value;
    }

    public void EnterSlowMotion(float TransitionDuration = 0.5f) => fade.Begin(fade.Value, 0f, TransitionDuration);

    public void ExitSlowMotion(float TransitionDuration = 0.5f) => fade.Begin(fade.Value, 1f, TransitionDuration);
}