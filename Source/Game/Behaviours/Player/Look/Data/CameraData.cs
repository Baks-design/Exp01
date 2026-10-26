namespace Game.Behaviours.Player;

public sealed class CameraData
{
    public float yaw;
    public float pitch;
    public float defaultFov;
    public bool initialized;

    public void Reset()
    {
        yaw = 0;
        pitch = 0;
        defaultFov = 0;
        initialized = false;  
    }
}