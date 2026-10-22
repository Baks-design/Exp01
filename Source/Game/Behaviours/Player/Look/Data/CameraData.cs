namespace Game.Behaviours.Player;

public sealed class CameraData
{
    public float yaw;
    public float pitch;
    public float defaultFov;
    public bool initialized;

    public void Reset()
    {
        yaw = 0f;
        pitch = 0f;
        defaultFov = 0f;
        initialized = false;
    }
}
