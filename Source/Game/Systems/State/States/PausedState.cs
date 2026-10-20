using FlaxEngine;
using Game.Core.Cursor;
using Game.Core.Helpers.Timer;

namespace Game.Systems.State;

public sealed class PausedState : IState
{
    private readonly CursorService cursor = new();
    private readonly TimeService time = new();
    
    public StateId Id => StateId.Paused;

    public bool CanEnter(StateId from) => from == StateId.InGame;

    public void OnEnter()
    {
        Debug.Log("Enter Paused");
        cursor.Apply(CursorState.Menu);
        time.PushTimeScale(0f);
        Engine.FocusGameViewport();
    }

    public void OnExit() => Debug.Log("Exit Paused");
}
