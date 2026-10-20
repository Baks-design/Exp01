using FlaxEngine;
using Game.Systems.Input;

namespace Game.Behaviours.Player;

public sealed class PlayerCamera : Script
{
    [Header("Strategies")]
    public ThirdPersonCameraStrategy ThirdPerson = new();
    public FirstPersonCameraStrategy FirstPerson = new();
    [Header("References")]
    public Camera CameraActor;
    public Actor TargetActor;
    private readonly InputService inputService = new();
    private ICameraStrategy strategy;
    private Vector2 mouseAngles = new(45f, 45f);

    public CameraPreset ActivePreset { get; private set; }

    public override void OnStart()
    {
        if (!CameraActor || !TargetActor)
        {
            Debug.LogError("CameraController: CameraActor or TargetActor is not assigned.");
            return;
        }

        SetCameraStrategy(ThirdPerson);
    }

    public override void OnUpdate()
    {
        if (!CameraActor || !TargetActor || strategy == null || ActivePreset == null) return;

        strategy.Update(inputService, CameraActor, TargetActor, ActivePreset, ref mouseAngles);
    }

    public void SetCameraStrategy(ICameraStrategy newStrategy)
    {
        if (newStrategy == null)
        {
            Debug.LogError("SwitchStrategy received null.");
            return;
        }

        strategy = newStrategy;
        mouseAngles = Vector2.Zero;
        RefreshActivePreset();
    }

    private void RefreshActivePreset()
    {
        var reference = strategy.PresetReference;
        if (reference == null || !reference.Asset)
        {
            Debug.LogError($"{strategy.GetType().Name} has no preset reference assigned.");
            ActivePreset = null;
            return;
        }

        ActivePreset = reference.Asset.CreateInstance<CameraPreset>();
    }
}