namespace Game.Core.Helpers.Coroutines;

public interface IYieldInstruction
{
    bool Step(float deltaTime);
}