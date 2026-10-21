using FlaxEngine;

namespace Game.Behaviours.Player;

public sealed class CameraPreset
{
    [Header("First Person")]
    public float FPMinPitch = -89f;
    public float FPMaxPitch = 89f;
    public float FPMouseSpeed = 0.1f;
    public float FPRotationSpeed = 0.5f;
    public float FPSmoothing = 12f;
    public float FPMinDistance = 10f;
    public float FPMaxDistance = 2000f;
    public float FPCollisionDistanceBias = 10f;
    public float FPHeightOffset = 160f;
    public float FPZoomSpeed = 1f;
    public float FPZoomMultipier = -20f;
    [Header("Third Person")]
    public float TPMinPitch = -89f;
    public float TPMaxPitch = 89f;
    public float TPMouseSpeed = 0.5f;
    public float TPSmoothing = 12f;
    public float TPDistance = 500f;
    public LayersMask TPCollisionLayers = LayersMask.Default;
    public float TPCollisionDistanceBias = 10f;
    public float TPZoomSpeed = 1f;
    public float TPZoomMultipier = -20f;
}
