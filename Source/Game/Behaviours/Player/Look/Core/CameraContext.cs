using FlaxEngine;
using Game.Systems.Inputs;

namespace Game.Behaviours.Player;

public sealed class CameraContext
{
    public InputService input;
    public Camera camera;
    public Actor cameraTarget;
    public Actor cameraPitch;
    public Actor cameraYaw;
    public CameraData cameraData;
    public float deltaTime; 
}