using FlaxEngine;

namespace Game.Behaviours.Player.Movement;

public sealed class ActorCameraOrientation(Actor camera) : ICameraOrientationSource
{
    readonly Actor camera = camera;

    public Vector3 Forward => FlattenYaw(camera.Transform.Forward);
    public Vector3 Right => FlattenYaw(camera.Transform.Right);

    static Vector3 FlattenYaw(Vector3 v)
    {
        v.Y = 0f;
        var len = v.Length;
        return len < 1e-5f ? Vector3.Forward : v / len;
    }
}
