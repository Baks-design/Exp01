using FlaxEngine;
using Game.Core.Cursor;
using Game.Core.Helpers.Services;
using Game.Core.Helpers.Timer;
using Game.Systems.Inputs;
using Game.Systems.Scene;

namespace Game.Systems.Bootstrap;

public sealed class GameBootstrap : GamePlugin
{
    public static ServiceLocator Services { get; private set; } = null!;

    public override void Initialize()
    {
        Services = new ServiceLocator();
        Services.RegisterFactory(() => new InputService());
        Services.RegisterFactory(() => new SceneService());
        Services.RegisterFactory(() => new TimeService());
        Services.RegisterFactory(() => new CursorService());
    }

    public override void Deinitialize()
    {
        Services?.Dispose();
        Services = null!;
    }
}