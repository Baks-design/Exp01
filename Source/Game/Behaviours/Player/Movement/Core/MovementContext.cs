using FlaxEngine;
using Game.Systems.Inputs;

namespace Game.Behaviours.Player;

public sealed class MovementContext
{
    public float deltaTime;
    public Actor cameraYaw;
    public PlayerCamera camera;
    public InputService input;
    public CharacterController controller;
    public PlayerMovementData movementData;
    public PlayerSound sound;
}