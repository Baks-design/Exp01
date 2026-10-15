using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game.Core.Helpers.Timer;

public sealed class TimeService : ITimeService
{
    readonly Stack<float> timeScaleStack = new();
    bool disposed;

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

    void ThrowIfDisposed() => ObjectDisposedException.ThrowIf(disposed, this);

    public TimeSnapshot Capture() => new(
        DeltaTime, FixedDeltaTime, TotalTime, UnscaledDeltaTime, FrameCount, TimeScale);

    public IDisposable PushTimeScale(float scale)
    {
        ThrowIfDisposed();

        var previous = TimeScale;
        timeScaleStack.Push(previous);
        TimeScale = scale;
        return new RestoreHandle(this, previous);
    }

    void Restore(float previous)
    {
        if (disposed) return;

        if (timeScaleStack.Count > 0) timeScaleStack.Pop();

        TimeScale = timeScaleStack.Count > 0 ? timeScaleStack.Peek() : previous;
    }

    public void Dispose()
    {
        if (disposed) return;
        disposed = true;

        if (timeScaleStack.Count > 0) TimeScale = timeScaleStack.Peek();
        timeScaleStack.Clear();
    }

    sealed class RestoreHandle(TimeService owner, float previous) : IDisposable
    {
        bool disposed;

        public void Dispose()
        {
            if (disposed) return;
            disposed = true;
            owner.Restore(previous);
        }
    }
}