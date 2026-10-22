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
    object activePreset;

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
        if (strategy == null || activePreset == null) return;

        strategy.ApplyCameraSmoothing(camera, cameraTarget, activePreset);
        strategy.ApplyMovement(input, controller, cameraTarget, activePreset, data, cameraData, sound);
        strategy.ApplyRotation(controller, activePreset, data, cameraData);
    }

    public override void OnDebugDraw()
    {
        if (!controller) return;

        DebugDraw.DrawWireCapsule(
            controller.Transform.Translation,
            controller.Transform.Orientation * Quaternion.Euler(90, 0, 0),
            controller.Radius,
            controller.Height,
            Color.Blue);
    }

    public void SetMovementStrategy<T>(IMovementStrategy<T> newStrategy) where T : class
    {
        if (newStrategy == null || strategy == newStrategy) return;

        strategy = newStrategy;
        data.Reset();

        var reference = newStrategy.PresetReference;
        if (reference == null || !reference.Asset)
        {
            Debug.LogError($"{newStrategy.GetType().Name} has no preset assigned.");
            activePreset = null;
            return;
        }

        activePreset = reference.Asset.CreateInstance<T>();
    }

    public void SetMovement(float horizontal, float vertical)
        => strategy?.AddMovement(data, horizontal, vertical);
}