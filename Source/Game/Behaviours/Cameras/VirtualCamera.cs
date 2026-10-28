using FlaxEngine;

namespace Game.Systems.Cameras;

public sealed class VirtualCamera : Script
{
    [Tooltip("Higher priority wins when multiple virtual cameras are enabled.")]
    public int Priority = 0;
    ICameraStrategy strategy;

    public ICameraStrategy GetStrategy
    {
        get
        {
            if (strategy != null) return strategy;

            foreach (var s in Actor.Scripts)
            {
                if (s is ICameraStrategy cs)
                {
                    strategy = cs;
                    break;
                }
            }

            //if (strategy == null)
             //   Debug.LogWarning($"[VirtualCamera] '{Actor.Name}' has no ICameraStrategy script attached.");

            return strategy;
        }
    }

    public override void OnEnable() => CameraBrain.Register(this);

    public override void OnDisable() => CameraBrain.Unregister(this);

    public override void OnDestroy()
    {
        CameraBrain.Unregister(this);
        strategy = null;
    }

    public void Evaluate(out Vector3 position, out Quaternion rotation)
    {
        var s = GetStrategy;
        if (s != null)
            s.Evaluate(this, out position, out rotation);
        else
        {
            position = Actor.Position;
            rotation = Actor.Orientation;
        }
    }
}