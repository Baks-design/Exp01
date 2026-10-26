using FlaxEngine;
using Game.Core.Cursor;
using Game.Core.Helpers.Timer;

namespace Game.Systems.State;

public sealed class PausedState : IState
{
    readonly CursorService cursorService = new();
    readonly TimeService timeService = new();

    public StateId Id => StateId.Paused;

    public bool CanEnter(StateId from) => from == StateId.InGame;

    public void OnEnter()
    {
        Debug.Log("Enter Pause Menu");
        cursorService.Apply(CursorState.Menu);
        timeService.Pause(); 
    }

    public void OnExit() { }
}
