using FlaxEngine;

namespace Game.Behaviours.Player;

public sealed class CameraPreset
{
    [Header("First Person")]
    public bool FPInvertY = true;
    public float FPLookSmoothing = 20;
    public Float2 FPPitch = new(-88, 88);
    public float FPSensitivity = 1;
    public float FPFOVLerpSpeed = 8;
    public float FPShakeDecay = 5;
    public float FPShakeScale = 1;
    public float FPFOVZoom = 30;

    [Header("Third Person")]
    public float TPSensitivity = 0.15f;
    public Float2 TPPitch = new(-60, 60);
    public float TPDistance = 400;
    public float TPMinDistance = 80;
    public float TPCollisionBias = 10;
    public LayersMask TPCameraCollisionLayers;
    public float TPFOVZoom = 50;
    public float TPFOVLerpSpeed = 8;
    public float TPShakeDecay = 5;
    public float TPShakeScale = 1;
    public float TPLockOnLerpSpeed = 10; 
}
