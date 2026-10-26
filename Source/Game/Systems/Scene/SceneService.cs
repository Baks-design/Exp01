using FlaxEngine;

namespace Game.Systems.Scene;

public class SceneService
{
    public void RequestTransition(SceneTransitionRequest request)
    {
        if (request == null)
        {
            Debug.LogError("Scene transition request is null.");
            return;
        }

        // Gameplay object edits (including scene transitions) must happen on the main thread.
        if (!Platform.IsInMainThread)
        {
            Debug.LogWarning($"Scene transition to '{request.Target}' requested off the main thread. Deferring to next update.");
            Scripting.InvokeOnUpdate(() => RequestTransition(request));
            return;
        }

        // Flax returns TRUE when the change FAILS (invalid asset, missing data, load error).
        var failed = Level.ChangeSceneAsync(request.Target);
        if (failed)
            Debug.LogError($"Failed to change scene to '{request.Target}'. Verify the target is a valid scene asset.");
    }
}