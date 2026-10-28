using Game.Core.Cursor;
using Game.Core.Helpers.Timer;

namespace Game.Systems.State;

public sealed class InGameState(CursorService cursor, TimeService time) : StateBase
{
    readonly CursorService cursor = cursor;
    readonly TimeService time = time;

    public override StateId Id => StateId.InGame;

    public override bool CanEnter(StateId from) => from is StateId.Loading or StateId.Paused;

    public override void OnEnter()
    {
        base.OnEnter();
        cursor.ApplyGameplay();
        time.Resume();
    }

    public override void OnUpdate(float dt) => cursor.ApplyGameplay();
}
