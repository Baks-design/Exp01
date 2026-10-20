using FlaxEngine;
using Game.Systems.State;

namespace Game.Systems.Bootstrap;

public sealed class BootstrapPlugin : GamePlugin
{
    public StateService StateService { get; } = new();

    public override void Initialize()
    {
        StateService.Initialize();
        if (StateService.CurrentId != StateId.InGame)
            Debug.LogError($"[Bootstrap] Failed to enter {StateId.InGame} state.");
    }

    public override void Deinitialize() => StateService.SetState(StateId.None);
}