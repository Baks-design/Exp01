using FlaxEngine;
using Game.Core.Cursor;
using Game.Core.Helpers.Timer;
using Game.Initialization;

namespace Game.Helpers;

public sealed class SystemTools : Script
{
    ICursorService cursor;
    ITimeService time;

    public override void OnStart()
    {
        var bootstrap = PluginManager.GetPlugin<GameBootstrapPlugin>();
        if (bootstrap != null)
        {
            cursor = bootstrap.Locator.Resolve<ICursorService>();
            time = bootstrap.Locator.Resolve<ITimeService>();
        }
        cursor?.Push(CursorState.Gameplay);
        time?.PushTimeScale(1f);
    }

    public override void OnUpdate()
    {
        if (Input.GetKeyUp(KeyboardKeys.P)) Engine.RequestExit();
    }
}
