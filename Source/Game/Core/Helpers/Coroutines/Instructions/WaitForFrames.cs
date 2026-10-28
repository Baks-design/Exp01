using FlaxEngine;

namespace Game.Core.Helpers.Coroutines;

public struct WaitForFrames(int frames = 1) : IYieldInstruction
{
    int remaining = Mathf.Max(1, frames);

    public bool Step(float deltaTime)
    {
        remaining--;
        return remaining <= 0;
    }
}