using System;
using System.Collections.Generic;
using FlaxEngine;
using Game.Core.Cursor;
using Game.Core.Helpers.Timer;
using Game.Systems.Bootstrap;

namespace Game.Systems.State;

public sealed class StateService
{
    readonly Dictionary<StateId, IState> states = [];

    public StateId CurrentId => Current?.Id ?? StateId.None;
    public IState Current { get; private set; }

    public void Initialize(bool startAtBooting = true)
    {
        if (states.Count > 0) throw new InvalidOperationException("StateService already initialized.");

        GameBootstrap.Services.TryResolve<CursorService>(out var cursor);
        GameBootstrap.Services.TryResolve<TimeService>(out var time);
        SetupStates(cursor, time);

        var entry = StateId.InGame; //startAtBooting ? StateId.Booting : StateId.MainMenu;
        if (!SetState(entry)) throw new InvalidOperationException($"Failed to enter initial state {entry}.");
    }

    void SetupStates(CursorService cursor, TimeService time)
    {
        states[StateId.InGame] = new InGameState(cursor, time);
        states[StateId.Paused] = new PausedState(cursor, time);
    }

    public bool SetState(StateId targetId)
    {
        if (!states.TryGetValue(targetId, out var next))
        {
            Debug.LogWarning($"[StateService] Unknown state: {targetId}");
            return false;
        }

        if (Current is { } cur && !next.CanEnter(cur.Id))
        {
            Debug.LogWarning($"[StateService] Illegal transition {cur.Id} -> {targetId}");
            return false;
        }

        Current?.OnExit();
        Current = next;
        Current.OnEnter();
        return true;
    }

    public void Update(float dt) => Current?.OnUpdate(dt);

    public void Shutdown()
    {
        Current?.OnExit();
        Current = null;
        states.Clear();
    }
}
