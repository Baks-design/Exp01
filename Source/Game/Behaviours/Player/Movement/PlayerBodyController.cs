using FlaxEngine;
using Game.Systems.Input;

namespace Game.Behaviours.Player.Movement;

public sealed class PlayerBodyController : Script
{
    [Header("Speed")]
    [Limit(1f, 2000f)] public float MaxSpeed { get; set; } = 50f;
    [Limit(0.1f, 60f)] public float MoveResponseRate { get; set; } = 20f;
    [Limit(0.1f, 60f)] public float AirResponseRate { get; set; } = 6f;
    [Header("Jump / Gravity")]
    [Limit(1f, 100f)] public float Gravity { get; set; } = 20f;
    [Limit(0.1f, 20f)] public float JumpHeight { get; set; } = 2f;
    [Limit(1f, 5f)] public float FallGravityMultiplier { get; set; } = 1.8f;
    [Limit(10f, 500f)] public float TerminalVelocity { get; set; } = 120f;
    [Limit(0f, 10f)] public float GroundStickSpeed { get; set; } = 2f;
    [Header("Feel")]
    [Limit(0f, 0.5f)] public float CoyoteTime { get; set; } = 0.12f;
    [Limit(0f, 0.5f)] public float JumpBufferTime { get; set; } = 0.12f;
    [Limit(0f, 1f)] public float JumpCooldown { get; set; } = 0.10f;
    [Limit(0, 4)] public int MaxAirJumps { get; set; } = 1;
    public required CharacterController Controller { get; set; }
    public required Actor BodyActor { get; set; }
    public required Actor CameraActor { get; set; }
    CharacterMotor motor;
    MovementInputSource inputSource;
    ICameraOrientationSource cameraOrientation;
    InputManager input;

    public override void OnStart()
    {
        if (!ResolveReferences()) { Enabled = false; return; }

        input ??= new();
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
        MoveResponseRate = MoveResponseRate,
        AirResponseRate = AirResponseRate,
        Gravity = Gravity,
        JumpHeight = JumpHeight,
        FallGravityMultiplier = FallGravityMultiplier,
        TerminalVelocity = TerminalVelocity,
        GroundStickSpeed = GroundStickSpeed,
        CoyoteTime = CoyoteTime,
        JumpBufferTime = JumpBufferTime,
        JumpCooldown = JumpCooldown,
        MaxAirJumps = MaxAirJumps,
    };

    public override void OnEnable()
    {
        input?.Subscribe(InputActions.Jump, inputSource.OnJumpAction);
        input?.Subscribe(InputActions.Sprint, inputSource.OnJumpAction);
    }

    public override void OnUpdate() => inputSource.Poll(Engine.HasGameViewportFocus);

    public override void OnFixedUpdate()
    {
        if (!Engine.HasGameViewportFocus || motor is null) return;

        var intent = BuildIntent(inputSource, cameraOrientation);
        var dt = 1f / Time.PhysicsFPS;
        motor.Step(intent, dt);
    }

    public override void OnDisable()
    {
        input?.Unsubscribe(InputActions.Jump, inputSource.OnJumpAction);
        input?.Unsubscribe(InputActions.Sprint, inputSource.OnJumpAction);
    }

    public override void OnDestroy()
    {
        input?.Dispose();
        input = null;
        motor = null;
        inputSource = null;
        cameraOrientation = null;
    }

    static MovementIntent BuildIntent(IInputSource src, ICameraOrientationSource cam)
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