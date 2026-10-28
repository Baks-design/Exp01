using System.Collections.Generic;
using FlaxEngine;

namespace Game.Systems.Cameras;

public sealed class CameraBrain : Script
{
    public Camera OutputCamera;
    ICameraStrategy activeStrategy;
    VirtualCamera activeVCam;
    static readonly List<VirtualCamera> vcams = [];
    static bool dirty = true;

    public override void OnStart()
    {
        if (!OutputCamera) OutputCamera = Actor.GetChild<Camera>();
        if (!OutputCamera) Debug.LogWarning("[CameraBrain] No OutputCamera found on this actor or its children.");

        SelectBest();
    }

    public override void OnLateFixedUpdate()
    {
        if (dirty)
        {
            SelectBest();
            dirty = false;
        }

        if (activeVCam == null) return;

        if (!OutputCamera)
        {
            OutputCamera = Actor.GetChild<Camera>();
            if (!OutputCamera) return;
        }

        activeVCam.Evaluate(out var pos, out var rot);
        OutputCamera.Position = pos;
        OutputCamera.Orientation = rot;
    }

    public override void OnDestroy()
    {
        activeStrategy = null;
        activeVCam = null;
    }

    void SelectBest()
    {
        VirtualCamera best = null;
        for (var i = 0; i < vcams.Count; i++)
        {
            var v = vcams[i];
            if (v == null || !v.Actor.IsActive) continue;

            if (best == null
                || v.Priority > best.Priority
                || (v.Priority == best.Priority && v.Actor.ID > best.Actor.ID))
                best = v;
        }

        if (best == activeVCam) return;

        activeStrategy?.OnDeactivate();

        activeVCam = best;
        activeStrategy = best?.GetStrategy;
        activeStrategy?.OnActivate(activeVCam);
    }

    public static void Register(VirtualCamera v)
    {
        if (v == null || vcams.Contains(v)) return;

        vcams.Add(v);
        dirty = true;
    }

    public static void Unregister(VirtualCamera v)
    {
        if (v == null) return;

        if (vcams.Remove(v)) dirty = true;
    }

    public static void MarkDirty() => dirty = true;
}