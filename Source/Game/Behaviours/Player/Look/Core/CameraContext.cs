using FlaxEngine;
using Game.Systems.Input;

namespace Game.Behaviours.Player;

public sealed class CameraContext
{
    public InputService input;
    public Actor cameraTarget;
    public Camera camera;
    public CameraData cameraData;
    public float deltaTime;
}
