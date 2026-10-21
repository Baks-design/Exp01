using FlaxEngine;
using Game.Systems.Input;

namespace Game.Behaviours.Player;

public interface ICameraStrategy
{
    public JsonAssetReference<CameraPreset> PresetReference { get; set; }

    public void Update(CameraData data, InputService input, Camera camera, Actor target, CameraPreset preset);
}
