namespace Game.Core.Helpers.Coroutines;

public struct WaitForFrames(int expectedFramesCount = 0) : IYieldInstruction
{
    readonly int expectedFramesCount = expectedFramesCount;
    int currentFrameCount = 0;

    public bool Step(float deltaTime)
    {
        currentFrameCount++;
        return currentFrameCount >= expectedFramesCount;
    }
}