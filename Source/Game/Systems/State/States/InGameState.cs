using FlaxEngine;
using Game.Core.Cursor;
using Game.Core.Helpers.Timer;

namespace Game.Systems.State;

public sealed class InGameState : IState
{
    readonly CursorService cursor = new();
    readonly TimeService time = new();
    
    public StateId Id => StateId.InGame;

    public bool CanEnter(StateId from) 
        => from is StateId.MainMenu ||
           from is StateId.Paused ||
           from is StateId.Loading;

    public void OnEnter()
    {
        Debug.Log("Enter InGame");
        cursor?.Apply(CursorState.Gameplay);
        time?.PushTimeScale(1f);
    }

    public void OnExit() => Debug.Log("Exit InGame");
}
