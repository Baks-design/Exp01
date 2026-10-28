using FlaxEngine;

namespace Game.Core.Helpers;

public class FPSChanger : Script
{
    public float TargetFPS = 60f;
    public float Step = 10f;

    public override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyboardKeys.ArrowUp))
        {
            TargetFPS += Step;
            ApplyFPS();
        }
        if (Input.GetKeyDown(KeyboardKeys.ArrowDown))
        {
            TargetFPS = Mathf.Max(1f, TargetFPS - Step);
            ApplyFPS();
        }
    }

    public override void OnDebugDraw()
    {
        var currentFps = Engine.FramesPerSecond;
        DebugDraw.DrawText(
            $"FPS: {currentFps:F1}",
            new Float2(10f, 10f),
            Color.White,
            20
        );
    }

    void ApplyFPS()
    {
        Time.UpdateFPS = TargetFPS;
        Time.PhysicsFPS = TargetFPS;
        Time.DrawFPS = TargetFPS;
        Debug.Log($"FPS changed to: {TargetFPS}");
    }
}