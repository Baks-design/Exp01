using FlaxEngine;

namespace Game.Behaviours.Player;

public sealed class CameraPreset
{
    public float RotationSpeed = 0.5f;
    public float ZoomSpeed = 1f;
    public float Distance = 1000f;
    public float MinDistance = 10f;
    public float MaxDistance = 2000f;
    public float CollisionDistanceBias = 10f;
    public LayersMask CollisionLayers = LayersMask.Default;
    public float FirstPersonHeightOffset = 160f;
}
