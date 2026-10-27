using FlaxEngine;

namespace Game.Systems.Cameras;

public interface ICameraStrategy
{
    void OnActivate(VirtualCamera camera);
    void OnDeactivate();
    void Evaluate(VirtualCamera camera, out Vector3 position, out Quaternion rotation);
}
