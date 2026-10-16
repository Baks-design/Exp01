using FlaxEngine;

namespace Game.Behaviours.Player.Look;

public sealed class PlayerCameraController : Script
{
    [Limit(0.01f, 10f)] public float SensitivityX = 1f;
    [Limit(0.01f, 10f)] public float SensitivityY = 1f;
    [Limit(0f, 2f)] public float AimScale = 1f;
    [Limit(0f, 2f)] public float InvertY = 1f;
    [Limit(-89f, 0f)] public float MinPitch = -89f;
    [Limit(0f, 89f)] public float MaxPitch = 89f;
    [Limit(1f, 25f)] public float DecayRate = 3.5f;
    [Limit(0f, 0.1f)] public float MaxFrameStep = 0.05f;
    public required Actor CameraActor;
    public required Actor YawActor;
    ExponentialSmoother Smoother;
    ILookSource LookSource;
    ICameraRig Rig;
    float smoothedPitch;
    float smoothedYaw;

    public override void OnStart()
    {
        if (!TryResolveCamera(out var camera)) return;

        BuildSettings();

        Smoother ??= new(BuildSettings().DecayRate);
        LookSource ??= new LookSource(AxisPair.Mouse);
        Rig ??= YawActor is { } yaw && yaw != camera
            ? new SplitCameraRig(yaw, camera, BuildSettings().MinPitch, BuildSettings().MaxPitch)
            : new LocalCameraRig(camera, BuildSettings().MinPitch, BuildSettings().MaxPitch);
        smoothedPitch = Rig.Pitch;
        smoothedYaw = Rig.Yaw;
    }

    bool TryResolveCamera(out Actor camera)
    {
        CameraActor ??= Actor.GetChild<Camera>();
        if (CameraActor is null)
        {
            Debug.LogWarning("[PlayerCameraController] No camera assigned or found.");
            camera = null;
            Enabled = false;
            return false;
        }
        camera = CameraActor;
        return true;
    }

    CameraSettings BuildSettings() => new()
    {
        SensitivityX = SensitivityX,
        SensitivityY = SensitivityY,
        AimScale = AimScale,
        InvertY = InvertY,
        MinPitch = MinPitch,
        MaxPitch = MaxPitch,
        DecayRate = DecayRate,
        MaxFrameStep = MaxFrameStep
    };

    public override void OnUpdate()
    {
        if (!Engine.HasGameViewportFocus) return;
        if (Rig is null || LookSource is null || Smoother is null) return;

        StepLook(Time.DeltaTime);
    }

    void StepLook(float deltaTime)
    {
        var source = LookSource;
        var smoother = Smoother;
        var rig = Rig;

        var dt = Mathf.Min(deltaTime, BuildSettings().MaxFrameStep);

        var raw = source.Read();
        var dx = raw.X * BuildSettings().SensitivityX * BuildSettings().AimScale;
        var dy = raw.Y * BuildSettings().SensitivityY * BuildSettings().AimScale * BuildSettings().InvertY;

        var targetPitch = rig.Pitch + dy;
        var targetYaw = rig.Yaw + dx;

        smoothedPitch = smoother.Step(rig.Pitch, targetPitch, dt);

        var yawDelta = Mathf.Wrap(targetYaw - rig.Yaw, -180f, 180f);
        var yawFactor = 1f - Mathf.Exp(-BuildSettings().DecayRate * dt);
        smoothedYaw = Mathf.Wrap(rig.Yaw + yawDelta * yawFactor, -180f, 180f);

        rig.Apply(new(smoothedPitch, smoothedYaw), dt);
    }

    public void ResetLook()
    {
        Smoother?.Reset();
        if (Rig is not null)
        {
            smoothedPitch = Rig.Pitch;
            smoothedYaw = Rig.Yaw;
        }
    }
}