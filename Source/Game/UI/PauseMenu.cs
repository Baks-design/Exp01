using System;
using FlaxEngine;
using Game.Core.Cursor;
using Game.Core.Helpers.Timer;
using Game.Systems.Input;

namespace Game.UI;

public sealed class PauseMenu : Script
{
    [Limit(1f, 10f)] public float TransitionDuration { get; set; } = 5f;
    TimeService time;
    InputManager input;
    CursorService cursor;
    SlowMotionHandler slowMotionHandler;
    IDisposable menuCursor;
    IDisposable gameplayCursor;
    bool isOpen;

    public override void OnStart()
    {
        input ??= new();
        cursor ??= new();
        gameplayCursor = cursor.Push(CursorState.Gameplay);
        time ??= new();
        slowMotionHandler ??= new();
        slowMotionHandler.Init();
    }

    public override void OnEnable() => input?.Subscribe(InputActions.Menu, TogglePause);

    public override void OnUpdate() => slowMotionHandler?.Update();

    public override void OnDisable() => input?.Unsubscribe(InputActions.Menu, TogglePause);

    public override void OnDestroy()
    {
        menuCursor?.Dispose();
        menuCursor = null;

        gameplayCursor?.Dispose();
        gameplayCursor = null;

        slowMotionHandler = null;

        input?.Dispose();
        cursor?.Dispose();
        time?.Dispose();
    }

    void TogglePause()
    {
        if (isOpen) CloseMenu();
        else OpenMenu();
    }

    void OpenMenu()
    {
        if (isOpen) return;
        isOpen = true;

        menuCursor = cursor.Push(CursorState.Menu);

        slowMotionHandler?.EnterSlowMotion(TransitionDuration);

        Engine.FocusGameViewport();
    }

    void CloseMenu()
    {
        if (!isOpen) return;
        isOpen = false;

        slowMotionHandler?.ExitSlowMotion(TransitionDuration);

        menuCursor?.Dispose();
        menuCursor = null;
    }
}