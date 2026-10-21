namespace Game.Behaviours.Player;

public sealed class CameraData
{
    public float yaw;
    public float pitch;

    public void Reset()
    {
        yaw = 0f;
        pitch = 0f;
    }
}
