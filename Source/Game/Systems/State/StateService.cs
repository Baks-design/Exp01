using System.Collections.Generic;

namespace Game.Systems.State;

// Use:
// OnState => stateService.Initialize();
// OnUpdate => stateService.SwitchState(StateId.)
public sealed class StateService
{
    readonly Dictionary<StateId, IState> states = [];
    IState currentState;

    public StateId CurrentId => currentState?.Id ?? StateId.None;

    public void Initialize()
    {
        SetupStates();
        SetState(StateId.InGame);
    }

    void SetupStates()
    {
        states.TryAdd(StateId.Booting, new BootingState());
        states.TryAdd(StateId.MainMenu, new MainMenuState());
        states.TryAdd(StateId.Loading, new LoadingState());
        states.TryAdd(StateId.InGame, new InGameState());
        states.TryAdd(StateId.Paused, new PausedState());
        states.TryAdd(StateId.GameOver, new GameOverState());
    }

    public bool SetState(StateId targetId)
    {
        if (!states.TryGetValue(targetId, out var next) ||
            currentState != null && !next.CanEnter(currentState.Id))
            return false;

        currentState?.OnExit();
        currentState = next;
        currentState?.OnEnter();

        return true;
    }
}