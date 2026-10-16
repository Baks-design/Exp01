using FlaxEngine;
using Game.Core.Cursor;
using Game.Core.Helpers.Timer;
using Game.Initialization;
using Game.Systems.Input;

namespace Game.UI;

public sealed class PauseMenu : Script
{
    ITimeService time;
    IInputService input;
    ICursorService cursor;
    bool isOpen;

    public override void OnStart()
    {
        var bootstrap = PluginManager.GetPlugin<GameBootstrapPlugin>();
        if (bootstrap != null)
        {
            cursor = bootstrap.Locator.Resolve<ICursorService>();
            input = bootstrap.Locator.Resolve<IInputService>();
            time = bootstrap.Locator.Resolve<ITimeService>();
        }
    }

    public override void OnEnable() => input?.Subscribe(InputActions.Menu, TogglePause);

    public override void OnDisable() => input?.Unsubscribe(InputActions.Menu, TogglePause);

    void TogglePause()
    {
        if (isOpen) CloseMenu();
        else OpenMenu();
    }

    void OpenMenu()
    {
        if (isOpen) return;
        isOpen = true;

        cursor?.Push(CursorState.Menu);
        time?.PushTimeScale(0f);
        Engine.FocusGameViewport();
    }

    void CloseMenu()
    {
        if (!isOpen) return;
        isOpen = false;

        time?.PushTimeScale(1f);
        cursor?.Push(CursorState.Gameplay);
    }
}