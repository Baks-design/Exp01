using FlaxEngine;

namespace Game.Systems.Cameras;

public sealed class VirtualCamera : Script
{
    public int Priority = 0;
    ICameraStrategy strategy;

    public ICameraStrategy GetStrategy => strategy;

    public override void OnStart()
    {
        foreach (var s in Actor.Scripts)
            if (s is ICameraStrategy cs) { strategy = cs; break; }
    }

    public void Evaluate(out Vector3 position, out Quaternion rotation)
    {
        if (strategy != null)
            strategy.Evaluate(this, out position, out rotation);
        else
        {
            position = Actor.Position;
            rotation = Actor.Orientation;
        }
    }
}