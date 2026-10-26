using FlaxEngine;

namespace Game.Core.Cursor;

public sealed class CursorService
{
    bool initialized;

    /// <summary>
    /// Gets the current cursor state.
    /// </summary>
    public CursorState Current { get; private set; }

    /// <summary>
    /// Applies the given cursor state to the screen.
    /// </summary>
    public void Apply(CursorState state)
    {
        if (initialized && Current == state) return;

        Screen.CursorVisible = state.Visible;
        Screen.CursorLock = state.LockMode;

        Current = state;
        initialized = true;
    }

    /// <summary>
    /// Convenience method to apply gameplay cursor (hidden + locked).
    /// </summary>
    public void ApplyGameplay() => Apply(CursorState.Gameplay);

    /// <summary>
    /// Convenience method to apply menu cursor (visible + unlocked).
    /// </summary>
    public void ApplyMenu() => Apply(CursorState.Menu);

    /// <summary>
    /// Toggles between gameplay and menu cursor states.
    /// </summary>
    /// <returns>The new applied state.</returns>
    public CursorState Toggle()
    {
        var next = Current == CursorState.Gameplay ? CursorState.Menu : CursorState.Gameplay;
        Apply(next);
        return next;
    }

    /// <summary>
    /// Resets cursor to a safe default (visible, unlocked).
    /// </summary>
    public void Reset() => Apply(CursorState.Menu);
}