
using FlaxEngine;

namespace Game.Systems.Cameras;

public sealed class FollowStrategy : Script, ICameraStrategy
{
    public Actor Target;
    public float Distance = 600;
    public float Height = 200;
    public float Smooth = 8;
    Vector3 currentPos;
    bool initialized;

    public void OnActivate(VirtualCamera camera) => initialized = false;

    public void OnDeactivate() { }

    public void Evaluate(VirtualCamera camera, out Vector3 position, out Quaternion rotation)
    {
        if (!Target)
        {
            position = camera.Actor.Position;
            rotation = camera.Actor.Orientation;
            return;
        }

        // Desired position: behind & above the target
        var desired = Target.Position - Target.Forward * Distance + Vector3.Up * Height;
        if (!initialized)
        {
            currentPos = desired;
            initialized = true;
        }
        else
            currentPos = Vector3.Lerp(currentPos, desired, 1f - Mathf.Exp(-Smooth * Time.DeltaTime));

        position = currentPos;
        rotation = Quaternion.LookAt(currentPos, Target.Position, Vector3.Up);
    }
}
