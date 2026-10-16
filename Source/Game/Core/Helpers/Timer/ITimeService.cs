namespace Game.Core.Helpers.Timer;

public interface ITimeService
{
    float DeltaTime { get; }
    float FixedDeltaTime { get; }
    float TotalTime { get; }
    float UnscaledDeltaTime { get; }
    ulong FrameCount { get; }
    float TimeScale { get; set; }

    void PushTimeScale(float scale);
}