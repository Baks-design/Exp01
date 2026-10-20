using FlaxEngine;

namespace Game.Systems.State;

public sealed class LoadingState : IState
{
    public StateId Id => StateId.Loading;

    public bool CanEnter(StateId from) => from is StateId.MainMenu || from is StateId.InGame;

    public void OnEnter() => Debug.Log("Enter Loading");

    public void OnExit() => Debug.Log("Exit Loading");
}
