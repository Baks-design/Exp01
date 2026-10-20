using FlaxEngine;

namespace Game.Core.Cursor;

public readonly record struct CursorState(bool Visible, CursorLockMode LockMode)
{
    public static readonly CursorState Gameplay = new(false, CursorLockMode.Clipped);
    public static readonly CursorState Menu = new(true, CursorLockMode.None);
}
