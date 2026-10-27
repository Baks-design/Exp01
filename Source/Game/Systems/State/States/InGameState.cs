using FlaxEngine;
using Game.Core.Cursor;
using Game.Core.Helpers.Timer;

namespace Game.Systems.State;

public sealed class InGameState : IState
{
    readonly CursorService cursorService = new();
    readonly TimeService timeService = new();

    public StateId Id => StateId.InGame;

    public bool CanEnter(StateId from) => from == StateId.Loading || from == StateId.Paused;

    public void OnEnter()
    {
        Debug.Log("Enter InGame");
        cursorService?.Apply(CursorState.Gameplay);
        timeService?.Resume();
    }

    public void OnExit() { }
}
