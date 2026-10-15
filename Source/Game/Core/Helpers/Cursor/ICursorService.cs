using System;

namespace Game.Core.Cursor;

public interface ICursorService
{
    CursorState Current { get; }
    
    void Apply(CursorState state);
    IDisposable Push(CursorState state);
}
