using FlaxEngine;

namespace Game.Core.Helpers.Timer;

public sealed class TimeService
{
    public static void PushTimeScale(float scale) => Time.TimeScale = scale;
}