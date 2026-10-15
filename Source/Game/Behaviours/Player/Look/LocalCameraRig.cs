using FlaxEngine;

namespace Game.Behaviours.Player.Look;

public sealed class LocalCameraRig : ICameraRig
{
    readonly Actor actor;
    readonly float minPitch;
    readonly float maxPitch;

    public float Pitch { get; private set; }
    public float Yaw { get; private set; }

    public LocalCameraRig(Actor actor, float minPitch, float maxPitch)
    {
        this.actor = actor;
        if (minPitch > maxPitch) (minPitch, maxPitch) = (maxPitch, minPitch);
        this.minPitch = minPitch;
        this.maxPitch = maxPitch;

        var e = actor.Orientation.EulerAngles;
        Pitch = Mathf.Clamp(e.X, minPitch, maxPitch);
        Yaw = Mathf.Wrap(e.Y, -180f, 180f);
    }

    public void Apply(in LookIntent intent, float deltaTime)
    {
        Pitch = Mathf.Clamp(intent.Pitch, minPitch, maxPitch);
        Yaw = Mathf.Wrap(intent.Yaw, -180f, 180f);

        var yawRot = Quaternion.RotationAxis(Float3.Up, Yaw * Mathf.DegreesToRadians);
        var pitchRot = Quaternion.RotationAxis(Float3.Right, Pitch * Mathf.DegreesToRadians);

        var t = actor.Transform;
        t.Orientation = yawRot * pitchRot;
        actor.Transform = t;
    }
}

