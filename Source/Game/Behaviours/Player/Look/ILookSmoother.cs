namespace Game.Behaviours.Player.Look;

public interface ILookSmoother
{
    float Step(float current, float target, float deltaTime);
    void Reset();
}

