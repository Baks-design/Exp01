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
    readonly MovementContext ctx = new();
    IMovementStrategy strategy;

    public override void OnStart()
    {
        if (!controller) { Debug.LogError("PlayerMovement: controller missing."); return; }
        if (!camera) { Debug.LogError("PlayerMovement: camera missing."); return; }

        ctx.input = input;
        ctx.controller = controller;
        ctx.cameraTarget = cameraTarget;
        ctx.camera = camera;
        ctx.movementData = data;
        ctx.cameraData = cameraData;
        ctx.sound = sound;
    }

    public override void OnUpdate() => data.jumpRequested = false;

    public override void OnFixedUpdate()
    {
        if (strategy == null) return;
        
        ctx.deltaTime = Time.DeltaTime;
        strategy.Execute(ctx);
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

    public void SetMovementStrategy(IMovementStrategy newStrategy)
    {
        if (newStrategy == null || strategy == newStrategy) return;

        strategy = newStrategy;
        data.Reset();
        switch (newStrategy)
        {
            case FirstPersonMovement fp:
                fp.SetPreset(fp.PresetReference ? fp.PresetReference.Asset?.CreateInstance<FirstPersonMovementPreset>() : null);
                break;
            case ThirdPersonMovement tp:
                tp.SetPreset(tp.PresetReference ? tp.PresetReference.Asset?.CreateInstance<ThirdPersonMovementPreset>() : null);
                break;
        }
    }

    public void SetMovement(float horizontal, float vertical)
    {
        data.horizontalVelocity += horizontal;
        data.velocity += vertical;
    }
}