using FlaxEngine;

namespace Game.Systems.State;

public sealed class GameOverState : IState
{
    public StateId Id => StateId.GameOver;

    public bool CanEnter(StateId from) => from == StateId.InGame;

    public void OnEnter() => Debug.Log("Enter GameOver");

    public void OnExit() => Debug.Log("Exit GameOver");
}