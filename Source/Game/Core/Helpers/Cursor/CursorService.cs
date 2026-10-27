using FlaxEngine;

namespace Game.Core.Cursor;

public sealed class CursorService
{
    bool initialized;

    public CursorState Current { get; private set; }

    public void Apply(CursorState state)
    {
        if (initialized && Current == state) return;

        Screen.CursorVisible = state.Visible;
        Screen.CursorLock = state.LockMode;

        Current = state;
        initialized = true;
    }

    public void ApplyGameplay() => Apply(CursorState.Gameplay);

    public void ApplyMenu() => Apply(CursorState.Menu);

    public CursorState Toggle()
    {
        var next = Current == CursorState.Gameplay ? CursorState.Menu : CursorState.Gameplay;
        Apply(next);
        return next;
    }

    public void Reset() => Apply(CursorState.Menu);
}