using FlaxEngine;

namespace Game.Core.Helpers.Timer;

public readonly record struct TimeSnapshot(
    float DeltaTime,
    float FixedDeltaTime,
    float TotalTime,
    float UnscaledDeltaTime,
    ulong FrameCount,
    float TimeScale)
{
    public bool IsPaused => Time.GamePaused;
    public bool IsTimeFrozen => TimeScale <= 0.0001f;

    public float Smoothing(float speed) => Mathf.Saturate(speed * DeltaTime);
}
