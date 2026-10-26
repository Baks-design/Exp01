
using FlaxEngine;

namespace Game.Systems.Cameras;

public sealed class FirstPersonStrategy : Script, ICameraStrategy
{
    public Actor HeadTarget;
    public float MouseSensitivity = 0.25f;
    public float MinPitch = -85f;
    public float MaxPitch = 85f;
    float yaw;
    float pitch;

    public void OnActivate(VirtualCamera camera)
    {
        // Read initial orientation from VirtualCamera when activating
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

        // Mouse input: X controls Yaw, Y controls Pitch
        var delta = Input.MousePositionDelta * MouseSensitivity;
        yaw += delta.X;
        pitch -= delta.Y; // Invert Y-axis for natural feel
        pitch = Mathf.Clamp(pitch, MinPitch, MaxPitch);

        // Camera position locked to head
        position = HeadTarget.Position;

        // Orientation determined by Pitch/Yaw
        rotation = Quaternion.Euler(pitch, yaw, 0);
    }
}