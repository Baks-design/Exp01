using FlaxEngine;
using Game.Core.Cursor;
using Game.Core.Helpers.Timer;

namespace Game.Systems.State;

public sealed class InGameState : IState
{
    public StateId Id => StateId.InGame;

    public bool CanEnter(StateId from) => from == StateId.Loading || from == StateId.Paused;

    public void OnEnter()
    {
        Debug.Log("Enter InGame");
        CursorService.Apply(CursorState.Gameplay);
        TimeService.PushTimeScale(1f);
    }

    public void OnExit() { }
}
