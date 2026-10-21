using FlaxEngine;

namespace Game.Core.Cursor;

public sealed class CursorService
{
    public static void Apply(CursorState state)
    {
        Screen.CursorVisible = state.Visible;
        Screen.CursorLock = state.LockMode;
    }
}
