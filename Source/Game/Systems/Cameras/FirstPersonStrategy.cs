using FlaxEngine;

namespace Game.Systems.Cameras;

public sealed class FirstPersonStrategy : Script, ICameraStrategy
{
    public Actor HeadTarget;
    public float MouseSensitivity = 0.25f;
    public float MinPitch = -85;
    public float MaxPitch = 85;
    float yaw;
    float pitch;

    public void OnActivate(VirtualCamera camera)
    {
        var euler = camera.Actor.Orientation.EulerAngles;
        pitch = euler.X;
        yaw = euler.Y;
    }

    public void OnDeactivate() { }

    public void Evaluate(VirtualCamera camera, out Vector3 position, out Quaternion rotation)
    {
        if (!HeadTarget)
        {
            position = camera.Actor.Position;
            rotation = camera.Actor.Orientation;
            return;
        }

        var delta = Input.MousePositionDelta * MouseSensitivity;
        yaw += delta.X;
        pitch -= delta.Y; 
        pitch = Mathf.Clamp(pitch, MinPitch, MaxPitch);

        position = HeadTarget.Position;

        rotation = Quaternion.Euler(pitch, yaw, 0);
    }
}