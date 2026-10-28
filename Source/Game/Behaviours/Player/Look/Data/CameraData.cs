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
        yaw = 0f;
        smoothedYaw = 0f;
        pitch = 0f;
        smoothedPitch = 0f;
    }
}