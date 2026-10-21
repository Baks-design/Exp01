using FlaxEngine;
using Game.Systems.Input;

namespace Game.Behaviours.Player;

public sealed class PlayerMovement : Script
{
    [Header("Strategies")]
    public ThirdPersonMovement thirdPerson = new();
    public FirstPersonMovement firstPerson = new();
    [Header("References")]
    public CharacterController controller;
    public Camera camera;
    readonly InputService input = new();
    readonly PlayerMovementData data = new();
    IMovementStrategy strategy;
    PlayerMovementPreset activePreset;

    public override void OnStart()
    {
        if (!controller)
        {
            Debug.LogError("PlayerMovement: CharacterController is not assigned.");
            return;
        }
        if (!camera)
        {
            Debug.LogError("PlayerMovement: Camera is not assigned.");
            return;
        }
    }

    public override void OnUpdate()
    {
        strategy.ApplyMovement(input, controller, camera, activePreset, data);
        strategy.ApplyRotation(controller, activePreset, data);
    }

    public void SetMovementStrategy(IMovementStrategy newStrategy)
    {
        if (newStrategy == null|| strategy == newStrategy) return;

        strategy = newStrategy;
        data.Reset();
        RefreshActivePreset(newStrategy);
    }

    void RefreshActivePreset(IMovementStrategy newStrategy)
    {
        var reference = newStrategy.PresetReference;
        if (reference == null)
        {
            activePreset = null;
            return;
        }
        else
            activePreset = reference.Instance;
    }
}