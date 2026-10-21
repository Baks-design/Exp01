using FlaxEngine;
using Game.Core.Cursor;
using Game.Core.Helpers.Timer;

namespace Game.Systems.State;

public sealed class PausedState : IState
{
    public StateId Id => StateId.Paused;

    public bool CanEnter(StateId from) => from == StateId.InGame;

    public void OnEnter()
    {
        Debug.Log("Enter Pause Menu");
        CursorService.Apply(CursorState.Menu);
        TimeService.PushTimeScale(0f);
    }

    public void OnExit() { }
}
