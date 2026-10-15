using FlaxEngine;

namespace Game.Behaviours.Player.Look;

public sealed class SplitCameraRig : ICameraRig
{
    readonly Actor yawActor;
    readonly Actor pitchActor;
    readonly float minPitch;
    readonly float maxPitch;

    public float Pitch { get; private set; }
    public float Yaw { get; private set; }

    public SplitCameraRig(Actor yawActor, Actor pitchActor, float minPitch, float maxPitch)
    {
        this.yawActor = yawActor;
        this.pitchActor = pitchActor;
        if (minPitch > maxPitch) (minPitch, maxPitch) = (maxPitch, minPitch);
        this.minPitch = minPitch;
        this.maxPitch = maxPitch;

        var e = pitchActor.LocalOrientation.EulerAngles;
        Pitch = Mathf.Clamp(e.X, minPitch, maxPitch);
        Yaw = Mathf.Wrap(yawActor.Orientation.EulerAngles.Y, -180f, 180f);
    }

    public void Apply(in LookIntent intent, float deltaTime)
    {
        Pitch = Mathf.Clamp(intent.Pitch, minPitch, maxPitch);
        Yaw = Mathf.Wrap(intent.Yaw, -180f, 180f);

        yawActor.Orientation = Quaternion.RotationAxis(Float3.Up, Yaw * Mathf.DegreesToRadians);
        pitchActor.LocalOrientation = Quaternion.RotationAxis(Float3.Right, Pitch * Mathf.DegreesToRadians);
    }
}

