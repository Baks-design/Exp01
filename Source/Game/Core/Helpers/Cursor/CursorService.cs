using FlaxEngine;
using Game.Core.Helpers.Coroutines;

namespace Game.Core.Cursor;

public sealed class CursorService
{
    bool initialized;
    CursorState pendingState;

    public CursorState Current { get; private set; }

    public void Apply(CursorState state)
    {
        if (initialized && Current == state && pendingState == default) return;

#if FLAX_EDITOR
        if (!Engine.HasGameViewportFocus && state.LockMode != CursorLockMode.None)
        {
            pendingState = state;
            Current = state;
            initialized = true;
            return;
        }
#endif

        Screen.CursorVisible = state.Visible;
        Screen.CursorLock = state.LockMode;

        Current = state;
        pendingState = default;
        initialized = true;
    }

    public void SyncFocus()
    {
#if FLAX_EDITOR
        if (pendingState != default && Engine.HasGameViewportFocus)
        {
            var state = pendingState;
            pendingState = default;
            Apply(state);
        }
#endif
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