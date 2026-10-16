using FlaxEngine;
using Game.Core.Cursor;
using Game.Core.Helpers.Timer;
using Game.Core.Patterns.Services;
using Game.Systems.Input;

namespace Game.Initialization;

// var bootstrap = PluginManager.GetPlugin<GameBootstrapPlugin>();
// if (bootstrap != null) saveSystem = bootstrap.Locator.Resolve<MySaveSystem>();
public sealed class GameBootstrapPlugin : GamePlugin
{
    public IServiceLocator Locator { get; private set; }

    public override void Initialize()
    {
        base.Initialize();

        IInputService inputService = new InputService();
        ICursorService cursorService = new CursorService();
        ITimeService timeService = new TimeService();

        Locator = new ServiceLocator();
        Locator.Register(inputService);
        Locator.Register(cursorService);
        Locator.Register(timeService);
    }

    public override void Deinitialize()
    {
        Locator = null;
        base.Deinitialize();
    }
}