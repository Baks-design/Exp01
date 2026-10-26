
using FlaxEngine;

namespace Game.Systems.Cameras;

public sealed class CameraBrain : Script
{
    public Camera OutputCamera;
    ICameraStrategy activeStrategy;
    VirtualCamera activeVCam;

    public override void OnStart()
    {
        if (!OutputCamera) OutputCamera = Actor.GetChild<Camera>();

        // Pick the highest-priority VirtualCamera at start
        SelectBest();
    }

    void SelectBest()
    {
        VirtualCamera best = null;

        foreach (var vcam in Level.GetScripts<VirtualCamera>())
        {
            if (vcam == null) continue;
            if (best == null || vcam.Priority > best.Priority) best = vcam;
        }
        if (best == activeVCam) return;

        activeStrategy?.OnDeactivate();

        activeVCam = best;
        activeStrategy = best?.GetStrategy();  // helper below
        activeStrategy?.OnActivate(activeVCam);
    }

    public override void OnUpdate()
    {
        // Hot-swap if the priority winner changed (e.g. triggers raised Priority)
        SelectBest();

        if (activeStrategy == null || !OutputCamera) return;

        activeStrategy.Evaluate(activeVCam, out var pos, out var rot);
        OutputCamera.Position = pos;
        OutputCamera.Orientation = rot;
    }
}
