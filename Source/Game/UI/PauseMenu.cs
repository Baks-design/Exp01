using FlaxEngine;
using Game.Systems.Input;
using Game.Systems.State;

namespace Game.UI;

public sealed class PauseMenu : Script
{
    readonly InputService input = new();
    readonly StateService state = new();
    bool isOpen;

    public override void OnEnable() => input?.Subscribe(InputActions.Menu, TogglePause);

    public override void OnDisable() => input?.Unsubscribe(InputActions.Menu, TogglePause);

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
        state?.SetState(StateId.Paused);
    }

    void CloseMenu()
    {
        if (!isOpen) return;

        isOpen = false;
        state?.SetState(StateId.InGame);
    }
}