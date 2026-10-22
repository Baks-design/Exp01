using FlaxEngine;

namespace Game.Behaviours.Player;

public sealed class ThirdPersonCameraPreset
{
    public float sensitivity = 0.15f;
    public Float2 minMaxPitch = new(-60, 60);
    public float distance = 400;
    public float minDistance = 80;
    public float collisionBias = 10;
    public LayersMask cameraCollisionLayers;
    public float fOVZoom = 50;
    public float fOVLerpSpeed = 8;
    public float shakeDecay = 5;
    public float shakeScale = 1;
    public float lockOnLerpSpeed = 10;
}
