using FlaxEngine;

namespace Game.Behaviours.Player;

public sealed class CameraData
{
    public float yaw;
    public float pitch;
    public float defaultFov;
    public bool initialized;
    public float shakeTimer;
    public float shakeIntensity;
    public float shakeSeed;
    public Vector3 basePosition;

    public void Reset()
    {
        yaw = 0;
        pitch = 0;
        shakeTimer = 0;
        shakeIntensity = 0;
        shakeSeed = 0;
        basePosition = Vector3.Zero;
    }
}
