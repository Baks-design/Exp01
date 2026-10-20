using FlaxEngine;

namespace Game.Systems.State;

public sealed class MainMenuState : IState
{
    public StateId Id => StateId.MainMenu;

    public bool CanEnter(StateId from) => from == StateId.Booting || from == StateId.GameOver;

    public void OnEnter() => Debug.Log("Enter MainMenu");

    public void OnExit() => Debug.Log("Exit MainMenu");
}
