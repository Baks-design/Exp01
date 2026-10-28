using Game.Core.Cursor;
using Game.Core.Helpers.Timer;

namespace Game.Systems.State;

public sealed class PausedState(CursorService cursor, TimeService time) : StateBase
{
    readonly CursorService cursor = cursor;
    readonly TimeService time = time;

    public override StateId Id => StateId.Paused;

    public override bool CanEnter(StateId from) => from is StateId.InGame;

    public override void OnEnter()
    {
        base.OnEnter();
        cursor.ApplyMenu();
        time.Pause();
    }
}
