using FlaxEngine;

namespace Game.Core.Cursor;

public sealed class CursorService 
{
    public void Apply(CursorState state)
    {        
        Screen.CursorVisible = state.Visible;
        Screen.CursorLock = state.LockMode;
    }
}
