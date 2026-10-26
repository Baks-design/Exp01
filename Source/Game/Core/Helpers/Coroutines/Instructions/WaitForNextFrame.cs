namespace Game.Core.Helpers.Coroutines;

/// <summary>
/// Waits for the next frame.
/// </summary>
public struct WaitForNextFrame : IYieldInstruction
{
    /// <inheritdoc />
    public readonly bool Step(float deltaTime) => true;
}