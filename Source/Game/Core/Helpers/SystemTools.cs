using FlaxEngine;
using Game.Systems.State;

namespace Game.Helpers;

public sealed class SystemTools : Script
{
    private readonly StateService state = new();

    public override void OnStart() => state.SetState(StateId.InGame);

    public override void OnUpdate()
    {
        if (Input.GetKeyUp(KeyboardKeys.P)) Engine.RequestExit();
    }
}
