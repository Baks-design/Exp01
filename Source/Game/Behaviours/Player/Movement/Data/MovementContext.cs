using FlaxEngine;
using Game.Systems.Inputs;

namespace Game.Behaviours.Player;

public sealed class MovementContext
{
    public InputService input;
    public CharacterController controller;
    public Camera camera;
    public PlayerMovementData movementData;
    public CameraData cameraData;
    public PlayerSound sound;
    public float deltaTime;
}
