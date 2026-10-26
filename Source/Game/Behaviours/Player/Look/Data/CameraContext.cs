using FlaxEngine;
using Game.Systems.Inputs;

namespace Game.Behaviours.Player;

public sealed class CameraContext
{
    public InputService input;
    public Camera camera;
    public CameraData cameraData;
    public float deltaTime;
}
