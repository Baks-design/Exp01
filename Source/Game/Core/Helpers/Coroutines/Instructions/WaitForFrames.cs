namespace Game.Core.Helpers.Coroutines;

/// <summary>
/// Represents a yield instruction to wait until a certain number of frames have passed.
/// </summary>
public struct WaitForFrames(int expectedFramesCount = 0) : IYieldInstruction
{
    readonly int expectedFramesCount = expectedFramesCount;
    int currentFrameCount = 0;

    /// <inheritdoc />
    public bool Step(float deltaTime)
    {
        currentFrameCount++;
        return currentFrameCount >= expectedFramesCount;
    }
}