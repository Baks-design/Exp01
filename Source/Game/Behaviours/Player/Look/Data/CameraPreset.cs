using FlaxEngine;

namespace Game.Behaviours.Player;

public sealed class CameraPreset
{
    [Header("First Person")]
    public float FPSensitivity = 1;

    [Header("Third Person")]
    public float MouseSensitivity = 100;
    public float CameraLag = 10;
    public float FOVZoom = 50;
    public Float2 PitchMinMax = new(-45, 45);
}
