
using FlaxEngine;

namespace Game.Systems.Cameras;

public sealed class OrbitStrategy : Script, ICameraStrategy
{
    public Actor Target;
    public float Radius = 800;
    public float MinPitch = -80;
    public float MaxPitch = 80;
    public float RotSpeed = 0.25f;
    float yaw;
    float pitch = 15;

    public void OnActivate(VirtualCamera camera) { }

    public void OnDeactivate() { }

    public void Evaluate(VirtualCamera camera, out Vector3 position, out Quaternion rotation)
    {
        if (Input.GetMouseButton(MouseButton.Right))
        {
            var delta = Input.MousePositionDelta;
            yaw += delta.X * RotSpeed;
            pitch -= delta.Y * RotSpeed;
            pitch = Mathf.Clamp(pitch, MinPitch, MaxPitch);
        }

        var center = Target ? Target.Position : camera.Actor.Position;
        var rot = Quaternion.Euler(pitch, yaw, 0);
        position = center + rot.Z * -Radius;
        rotation = Quaternion.LookAt(position, center, Vector3.Up);
    }
}