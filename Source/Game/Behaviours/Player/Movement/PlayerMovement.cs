using FlaxEngine;
using Game.Systems.Inputs;

namespace Game.Behaviours.Player;

public sealed class PlayerMovement : Script
{
    [Header("References")]
    public CharacterController controller;
    public PlayerSound sound;
    public PlayerCamera playerCamera;
    public Actor cameraYaw;

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
        if (!cameraYaw) { Debug.LogError("PlayerMovement: cameraYaw missing."); return; }
        if (!playerCamera) { Debug.LogError("PlayerMovement: playerCamera missing."); return; }

        ctx.input = input;
        ctx.controller = controller;
        ctx.cameraYaw = cameraYaw;
        ctx.camera = playerCamera;  
        ctx.movementData = MovementData;
        ctx.sound = sound;
    }

    public override void OnFixedUpdate()
    {
        if (strategy == null) return;

        var dt = Time.PhysicsFPS;
        ctx.deltaTime = dt;
        strategy.Execute(ctx, dt);
    }

    public override void OnDebugDraw() =>
        DebugDraw.DrawWireCapsule(
            controller.Transform.Translation,
            controller.Transform.Orientation * Quaternion.Euler(90f, 0f, 0f),
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

    public void AddImpulse(Vector3 impulse) => MovementData.prevVelocity += impulse;
}