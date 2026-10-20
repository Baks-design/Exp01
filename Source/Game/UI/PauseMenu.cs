using FlaxEngine;
using Game.Systems.Input;
using Game.Systems.State;

namespace Game.UI;

public sealed class PauseMenu : Script
{
    private readonly InputService input = new();
    private readonly StateService state = new();
    private bool isOpen;

    public override void OnEnable() => input.Subscribe(InputActions.Menu, TogglePause);

    public override void OnDisable() => input.Unsubscribe(InputActions.Menu, TogglePause);

    private void TogglePause()
    {
        Debug.Log("asasa");
        if (isOpen)
            CloseMenu();
        else
            OpenMenu();
    }

    private void OpenMenu()
    {
        if (isOpen) return;

        isOpen = true;
        state.SetState(StateId.Paused);
    }

    private void CloseMenu()
    {
        if (!isOpen) return;

        isOpen = false;
        state.SetState(StateId.InGame);
    }
}