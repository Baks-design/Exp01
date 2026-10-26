using FlaxEngine;
using Game.Systems.Inputs;

namespace Game.Behaviours.Player;

public sealed class PlayerMovement : Script
{
    [Header("References")]
    public CharacterController controller;
    public PlayerSound sound;
    public PlayerCamera playerCamera;
    public Camera camera;
    [Header("Strategies")]
    public FirstPersonMovement firstPerson = new();
    public ThirdPersonMovement thirdPerson = new();
    readonly InputService input = new();
    readonly MovementContext ctx = new();
    IMovementStrategy strategy;

    [ReadOnly] public PlayerMovementData MovementData { get; set; } = new();

    public override void OnStart()
    {
        if (!controller) { Debug.LogError("PlayerMovement: controller missing."); return; }
        if (camera == null) camera = Level.FindActor<Camera>();

        ctx.input = input;
        ctx.controller = controller;
        ctx.camera = camera;
        ctx.movementData = MovementData;
        ctx.cameraData = playerCamera.CameraData;
        ctx.sound = sound;
    }

    public override void OnUpdate() => MovementData.jumpRequested = false;

    public override void OnFixedUpdate() => strategy.Execute(ctx);

    public override void OnDebugDraw() =>
        DebugDraw.DrawWireCapsule(
            controller.Transform.Translation,
            controller.Transform.Orientation * Quaternion.Euler(90, 0, 0),
            controller.Radius,
            controller.Height,
            Color.Blue);

    public void SetMovementStrategy(IMovementStrategy newStrategy)
    {
        if (strategy == newStrategy) return;

        strategy = newStrategy;
        MovementData.Reset();

        switch (newStrategy)
        {
            case FirstPersonMovement fp:
                fp.SetPreset(fp.PresetReference
                    ? fp.PresetReference.Asset?.CreateInstance<FirstPersonMovementPreset>()
                    : null);
                break;
            case ThirdPersonMovement tp:
                tp.SetPreset(tp.PresetReference
                    ? tp.PresetReference.Asset?.CreateInstance<ThirdPersonMovementPreset>()
                    : null);
                break;
        }
    }

    public void SetMovement(float horizontal, float vertical)
    {
        MovementData.horizontalVelocity += horizontal;
        MovementData.verticalVelocity += vertical;
    }
}