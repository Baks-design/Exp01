namespace Game.Core.Helpers.Coroutines;

public interface IYieldInstruction
{
    public bool Step(float deltaTime);
}