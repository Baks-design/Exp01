using FlaxEngine;

namespace Game.Systems.State;

public sealed class BootingState : IState
{
    public StateId Id => StateId.Booting;

    public bool CanEnter(StateId from) => from == StateId.None;

    public void OnEnter() => Debug.Log("Enter Booting");

    public void OnExit() => Debug.Log("Exit Booting");
}
