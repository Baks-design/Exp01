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
    public Actor charObj;
    public Actor cameraTarget;
    public Camera camera;
    public PlayerSound sound;
    readonly InputService input = new();
    readonly PlayerMovementData data = new();
    readonly CameraData cameraData = new();
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

    public override void OnUpdate() => data.jumpRequested = false;

    public override void OnFixedUpdate()
    {
        strategy.ApplyCameraSmoothing(camera, cameraTarget, activePreset);
        strategy.ApplyMovement(input, controller, cameraTarget, activePreset, data, cameraData, sound);
        strategy.ApplyRotation(controller, activePreset, data, charObj, cameraData);
    }

    public override void OnDebugDraw()
        => DebugDraw.DrawWireCapsule(
            controller.Transform.Translation,
            controller.Transform.Orientation * Quaternion.Euler(90, 0, 0),
            controller.Radius,
            controller.Height,
            Color.Blue);

    public void SetMovementStrategy(IMovementStrategy newStrategy)
    {
        if (newStrategy == null || strategy == newStrategy) return;

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

    public void SetMovement(float horizontal, float vertical)
        => strategy.AddMovement(data, horizontal, vertical);
}