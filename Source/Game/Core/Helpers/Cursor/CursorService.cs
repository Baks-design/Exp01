using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game.Core.Cursor;

public sealed class CursorService : ICursorService
{
    readonly Stack<CursorState> stack = new();
    bool disposed;

    public CursorState Current => stack.Count > 0
        ? stack.Peek()
        : new CursorState(Screen.CursorVisible, Screen.CursorLock);

    void ThrowIfDisposed() => ObjectDisposedException.ThrowIf(disposed, this);

    public void Apply(CursorState state)
    {
        ThrowIfDisposed();
        
        Screen.CursorVisible = state.Visible;
        Screen.CursorLock = state.LockMode;
    }

    public IDisposable Push(CursorState state)
    {
        ThrowIfDisposed();

        var previous = Current;
        stack.Push(state);
        Apply(state);

        return new RestoreHandle(this, previous);
    }

    void Restore(CursorState previous)
    {
        if (disposed) return;

        if (stack.Count > 0) stack.Pop();

        if (stack.Count == 0) Apply(previous);
        else Apply(stack.Peek());
    }

    public void Dispose()
    {
        if (disposed) return;
        disposed = true;

        if (stack.Count > 0)
        {
            var bottom = stack.Peek();
            Apply(bottom);
        }

        stack.Clear();
    }

    sealed class RestoreHandle(CursorService owner, CursorState previous) : IDisposable
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
