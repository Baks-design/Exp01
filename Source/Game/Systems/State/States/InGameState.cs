using FlaxEngine;
using Game.Core.Cursor;
using Game.Core.Helpers.Timer;

namespace Game.Systems.State;

public sealed class InGameState : IState
{
    private readonly CursorService cursor = new();
    private readonly TimeService time = new();

    public StateId Id => StateId.InGame;

    public bool CanEnter(StateId from) => from == StateId.Loading || from == StateId.Paused;

    public void OnEnter()
    {
        Debug.Log("Enter InGame");
        cursor.Apply(CursorState.Gameplay);
        time.PushTimeScale(1f);
    }

    public void OnExit() => Debug.Log("Exit InGame");
}
