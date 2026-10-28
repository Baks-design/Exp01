using FlaxEngine;
using Game.Systems.Inputs;

namespace Game.Behaviours.Player;

public sealed class PlayerCamera : Script
{
    [Header("References")]
    public Camera camera;
    public Actor cameraPitch;
    public Actor cameraYaw;
    public Actor cameraTarget;

    [Header("Strategies")]
    public FirstPersonCamera firstPerson = new();
    public ThirdPersonCamera thirdPerson = new();

    readonly InputService input = new();
    readonly CameraContext ctx = new();
    ICameraStrategy currentStrategy;
    private bool wasPausedLastFrame;

    [ReadOnly] public CameraData CameraData { get; set; } = new();

    public override void OnStart()
    {
        if (camera == null) camera = Level.FindActor<Camera>();

        ctx.input = input;
        ctx.camera = camera;
        ctx.cameraPitch = cameraPitch;
        ctx.cameraYaw = cameraYaw;
        ctx.cameraTarget = cameraTarget;
        ctx.cameraData = CameraData;

        if (!CameraData.initialized)
        {
            CameraData.defaultFov = camera.FieldOfView;
            CameraData.initialized = true;
        }

        if (currentStrategy == null) SetCameraStrategy(firstPerson);
    }

    public override void OnLateFixedUpdate()
    {
        if (currentStrategy == null || ctx.camera == null) return;

        if (Time.TimeScale == 0f) return;
        if (wasPausedLastFrame)
        {
            wasPausedLastFrame = false;
            return;
        }
        
        ctx.deltaTime = Time.PhysicsFPS;
        currentStrategy.Execute(ctx);
    }

    public void SetCameraStrategy(ICameraStrategy newStrategy)
    {
        if (newStrategy == null || currentStrategy == newStrategy) return;

        currentStrategy = newStrategy;
        CameraData.Reset();
        currentStrategy.Configure();
    }

    public void SetRotation(float pitch, float yaw, bool snap = false)
    {
        var range = CurrentPitchRange();
        Float2 CurrentPitchRange() => currentStrategy switch
        {
            FirstPersonCamera => new Float2(-60f, 60f),
            ThirdPersonCamera => new Float2(-60f, 60f),
            _ => new Float2(-89f, 89f)
        };
        CameraData.pitch = Mathf.Clamp(CameraData.pitch + pitch, range.X, range.Y);
        CameraData.yaw += yaw;

        if (snap)
        {
            CameraData.smoothedPitch = CameraData.pitch;
            CameraData.smoothedYaw = CameraData.yaw;
        }
    }
}