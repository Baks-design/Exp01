using FlaxEngine;
using Game.Core.Cursor;
using Game.Core.Helpers.Timer;
using Game.Systems.State;

namespace Game.Helpers;

public sealed class SystemTools : Script
{
    readonly CursorService cursor = new();
    readonly TimeService time = new();
    readonly StateService state = new();

    public override void OnStart()
    {
        state?.SetState(StateId.InGame);
        cursor?.Apply(CursorState.Gameplay);
        time?.PushTimeScale(1f);
    }

    public override void OnUpdate()
    {
        if (Input.GetKeyUp(KeyboardKeys.P)) Engine.RequestExit();
    }
}
