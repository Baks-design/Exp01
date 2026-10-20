using FlaxEngine;

namespace Game.Core.Helpers.Timer;

public sealed class TimeService 
{
    public float DeltaTime => Time.DeltaTime;
    public float FixedDeltaTime => Time.PhysicsFPS > 0f ? 1f / Time.PhysicsFPS : 0f;
    public float TotalTime => Time.GameTime;
    public float UnscaledDeltaTime => Time.UnscaledDeltaTime;
    public ulong FrameCount => Engine.FrameCount;
    public float TimeScale
    {
        get => Time.TimeScale;
        set => Time.TimeScale = value;
    }

    public void PushTimeScale(float scale) => TimeScale = scale;
}