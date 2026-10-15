using System;

namespace Game.Core.Helpers.Timer;

public interface ITimeService : IDisposable
{
    float DeltaTime { get; }
    float FixedDeltaTime { get; }
    float TotalTime { get; }
    float UnscaledDeltaTime { get; }
    ulong FrameCount { get; }
    float TimeScale { get; set; }

    TimeSnapshot Capture();
    IDisposable PushTimeScale(float scale);
}
