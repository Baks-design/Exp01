namespace Game.Behaviours.Player;

public sealed class CameraData
{
    public float yaw;
    public float smoothedYaw;
    public float pitch;
    public float smoothedPitch;
    public float defaultFov;
    public bool initialized;

    public void Reset()
    {
        yaw = 0;
        smoothedYaw = 0;
        pitch = 0;
        smoothedPitch = 0;
        defaultFov = 0;
        initialized = false;
    }
}