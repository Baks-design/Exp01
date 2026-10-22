using FlaxEngine;

namespace Game.Behaviours.Player;

public sealed class FirstPersonCameraPreset
{
    public bool invertY = true;
    public float lookSmoothing = 20;
    public Float2 minMaxPitch = new(-88, 88);
    public float sensitivity = 1;
    public float fOVLerpSpeed = 8;
    public float shakeDecay = 5;
    public float shakeScale = 1;
    public float fOVZoom = 30;
}
