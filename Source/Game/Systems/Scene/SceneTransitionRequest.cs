using System;
using FlaxEngine;

namespace Game.Systems.Scene;

using System;
using FlaxEngine;

public sealed class SceneTransitionRequest
{
    SceneReference target;

    public SceneReference Target
    {
        get => target;
        set
        {
            if (value.ID == Guid.Empty)
                throw new ArgumentException("Scene reference must contain a valid asset ID.", nameof(value));

            target = value;
        }
    }

    public SceneTransitionRequest(SceneReference target) => Target = target;
}