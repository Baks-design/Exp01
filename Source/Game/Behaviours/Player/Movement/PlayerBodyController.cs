using FlaxEngine;
using Game.Initialization;
using Game.Systems.Input;

namespace Game.Behaviours.Player.Movement;

public sealed class PlayerBodyController : Script
{
    [Header("Speed")]
    [Limit(1f, 100f)] public float MaxSpeed = 50f;
    [Limit(1f, 5f)] public float SpeedMultiplier = 2f;
    [Limit(0.1f, 60f)] public float MoveResponseRate = 20f;
    [Limit(0.1f, 60f)] public float AirResponseRate = 6f;
    [Header("Jump / Gravity")]
    [Limit(0.1f, 100f)] public float JumpMultiplier = 2f;
    [Limit(1f, 5f)] public float FallGravityMultiplier = 1.8f;
    [Limit(10f, 500f)] public float TerminalVelocity = 120f;
    [Limit(0f, 10f)] public float GroundStickSpeed = 2f;
    [Header("Feel")]
    [Limit(0f, 0.5f)] public float CoyoteTime = 0.12f;
    [Limit(0f, 0.5f)] public float JumpBufferTime = 0.12f;
    [Limit(0f, 1f)] public float JumpCooldown = 0.10f;
    [Limit(0, 4)] public int MaxAirJumps = 2;
    public required CharacterController Controller;
    public required Actor BodyActor;
    public required Actor CameraActor;
    CharacterMotor motor;
    MovementInputSource inputSource;
    ICameraOrientationSource cameraOrientation;
    IInputService input;

    public override void OnStart()
    {
        if (!ResolveReferences()) { Enabled = false; return; }

        var bootstrap = PluginManager.GetPlugin<GameBootstrapPlugin>();
        if (bootstrap != null) input = bootstrap.Locator.Resolve<IInputService>();
    
        inputSource ??= new(input);
        cameraOrientation ??= new ActorCameraOrientation(CameraActor);
        motor ??= new(Controller, BuildSettings());

        BodyActor.StaticFlags = StaticFlags.None;
    }

    bool ResolveReferences()
    {
        Controller ??= Actor.Parent as CharacterController;
        if (Controller is null)
        {
            Debug.LogWarning($"[{nameof(PlayerBodyController)}] No CharacterController on parent.");
            return false;
        }
        BodyActor ??= Actor.Parent;
        if (BodyActor is null)
        {
            Debug.LogWarning($"[{nameof(PlayerBodyController)}] No BodyActor.");
            return false;
        }
        CameraActor ??= Actor.GetChild<Camera>();
        if (CameraActor is null)
        {
            Debug.LogWarning($"[{nameof(PlayerBodyController)}] No CameraActor.");
            return false;
        }
        return true;
    }

    MotorSettings BuildSettings() => new()
    {
        MaxSpeed = MaxSpeed,
        SpeedMultiplier= SpeedMultiplier,
        MoveResponseRate = MoveResponseRate,
        AirResponseRate = AirResponseRate,
        JumpMultiplier = JumpMultiplier,
        FallGravityMultiplier = FallGravityMultiplier,
        TerminalVelocity = TerminalVelocity,
        GroundStickSpeed = GroundStickSpeed,
        CoyoteTime = CoyoteTime,
        JumpBufferTime = JumpBufferTime,
        JumpCooldown = JumpCooldown,
        MaxAirJumps = MaxAirJumps,
    };

    public override void OnEnable() => input.Subscribe(InputActions.Jump, inputSource.OnJumpAction);

    public override void OnUpdate() => inputSource.Poll(Engine.HasGameViewportFocus);

    public override void OnFixedUpdate()
    {
        if (!Engine.HasGameViewportFocus) return;

        var intent = BuildIntent(inputSource, cameraOrientation);
        var time = 1f / Time.PhysicsFPS;
        motor.Step(intent, time);
    }

    public override void OnDisable() => input.Unsubscribe(InputActions.Jump, inputSource.OnJumpAction);

    static MovementIntent BuildIntent(MovementInputSource src, ICameraOrientationSource cam)
    {
        var raw = src.MoveAxis;
        var magnitude = raw.Length;
        if (magnitude < 0.05f)
            return new MovementIntent
            {
                Direction = Vector3.Zero,
                JumpPressed = src.JumpPressedThisFrame,
                SprintHeld = src.SprintHeld,
            };
        if (magnitude > 1f) raw /= magnitude;

        var worldDir = cam.Forward * raw.Y + cam.Right * raw.X;

        return new MovementIntent
        {
            Direction = worldDir,
            JumpPressed = src.JumpPressedThisFrame,
            SprintHeld = src.SprintHeld,
        };
    }
}