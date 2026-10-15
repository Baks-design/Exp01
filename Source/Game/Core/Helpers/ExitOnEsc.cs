using FlaxEngine;

namespace Game.Helpers;

public class ExitOnEsc : Script
{
    public override void OnUpdate()
    {
        if (Input.GetKeyUp(KeyboardKeys.P)) Engine.RequestExit();
    }
}
