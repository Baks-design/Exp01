namespace Game.Core.Helpers.Coroutines;

public struct WaitForNextFrame : IYieldInstruction
{
    public readonly bool Step(float deltaTime) => true;
}