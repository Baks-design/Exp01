using FlaxEngine;
using Game.Systems.State;

namespace Game.Systems.Bootstrap;

public sealed class BootstrapPlugin : GamePlugin
{
    public override void Initialize()
    {
        base.Initialize();

        var stateService = new StateService();
        stateService.Initialize();
    }

    public override void Deinitialize() => base.Deinitialize();
}