using FlaxEngine;

namespace Game.Behaviours.Player;

public sealed class TopDownCameraPreset 
{
    public float distance = 800f;             
    public float fixedPitch = -70f;           
    public Vector3 targetOffset = new(0f, 50f, 0f); 
    public float yawSensitivity = 1.5f;
    public float smoothing = 12f;
    public float collisionSmooth = 10f;
    public float collisionBias = 20f;
    public float minDistance = 100f;
    public LayersMask cameraCollisionLayers = LayersMask.Default;
    public float fOVZoom = 1.2f;
    public float fOVLerpSpeed = 8f;
}