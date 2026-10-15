namespace Game.Behaviours.Player.Look;

public interface ICameraRig
{
    float Pitch { get; }
    float Yaw { get; }

    void Apply(in LookIntent intent, float deltaTime);
}

