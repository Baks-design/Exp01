using FlaxEngine;
using Game.Systems.Bootstrap;
using Game.Systems.Inputs;
using Game.Systems.State;

namespace Game.UI;

public sealed class PauseMenu : Script
{
    StateManager stateInstance;
    InputService inputService;
    bool isOpen;

    public override void OnStart()
    {
        GameBootstrap.Services.TryResolve(out inputService);
        stateInstance = StateManager.Instance;
    }

    public override void OnEnable() => inputService.Subscribe(InputActions.Menu, TogglePause);

    public override void OnDisable() => inputService.Unsubscribe(InputActions.Menu, TogglePause);

    void TogglePause()
    {
        if (isOpen)
            CloseMenu();
        else
            OpenMenu();
    }

    void OpenMenu()
    {
        if (isOpen) return;

        isOpen = true;
        stateInstance.PauseGame();
    }

    void CloseMenu()
    {
        if (!isOpen) return;

        isOpen = false;
        stateInstance.ResumeGame();
    }
}