using System;
using FlaxEngine;

namespace Game.Systems.Scene;

public sealed class SceneTransitionRequest(SceneReference target)
{
    SceneReference target = target;

    public SceneReference Target
    {
        get => target;
        set => target = value.ID != Guid.Empty
            ? value
            : throw new ArgumentException("Scene reference must be valid.");
    }
}
