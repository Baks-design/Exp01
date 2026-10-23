using FlaxEngine;

namespace Game.Systems.Scene;

// Use:
// OnUpdate => sceneService = new SceneService();
public sealed class SceneService
{
    public static void RequestTransition(SceneTransitionRequest request)
    {
        if (!Level.ChangeSceneAsync(request.Target))
            Debug.LogError($"Failed to change scene to {request.Target}");
    }
}