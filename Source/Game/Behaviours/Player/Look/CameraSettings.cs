namespace Game.Behaviours.Player.Look;

public readonly record struct CameraSettings
{
    public required float SensitivityX { get; init; }
    public required float SensitivityY { get; init; }
    public required float AimScale { get; init; }
    public required float InvertY { get; init; }
    public required float MinPitch { get; init; }
    public required float MaxPitch { get; init; }
    public required float DecayRate { get; init; }
    public required float MaxFrameStep { get; init; }
}