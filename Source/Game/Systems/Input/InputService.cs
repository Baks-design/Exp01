using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game.Systems.Input;

// Use:
// inputService = new InputService();
public sealed class InputService
{
    readonly Dictionary<InputActions, InputEvent> events = [];
    readonly Dictionary<GameAxis, InputAxis> axes = [];
    bool disposed;

    public Float2 GetDelta => FlaxEngine.Input.MousePositionDelta;

    void ThrowIfDisposed() => ObjectDisposedException.ThrowIf(disposed, this);

    public void Subscribe(InputActions action, Action callback)
    {
        ThrowIfDisposed();
        ArgumentNullException.ThrowIfNull(callback);

        if (!events.TryGetValue(action, out var inputEvent))
        {
            inputEvent = new(action.Name);
            events[action] = inputEvent;
        }

        inputEvent.Pressed += callback;
    }

    public void Unsubscribe(InputActions action, Action callback)
    {
        if (disposed || callback is null) return;

        if (events.TryGetValue(action, out var inputEvent)) inputEvent.Pressed -= callback;
    }

    public void Dispose()
    {
        if (disposed) return;
        disposed = true;

        foreach (var e in events.Values) e.Dispose();

        events.Clear();
    }

    public bool GetAction(InputActions action)
    {
        ThrowIfDisposed();
        return FlaxEngine.Input.GetAction(action.Name);
    }

    public float GetAxis(GameAxis axis, bool rawValue = true)
    {
        ThrowIfDisposed();

        if (!axes.TryGetValue(axis, out var engineAxis))
        {
            engineAxis = new(axis.Name);
            axes[axis] = engineAxis;
        }

        return rawValue ? engineAxis.ValueRaw : engineAxis.Value;
    }
}
