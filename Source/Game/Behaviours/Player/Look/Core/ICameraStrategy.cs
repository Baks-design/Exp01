using FlaxEngine;
using Game.Systems.Input;

namespace Game.Behaviours.Player;

public interface ICameraStrategy
{
    public JsonAssetReference<CameraPreset> PresetReference { get; set; }

    public void Update(
        InputService inputService, Camera camera, Actor targetActor,
        CameraPreset activePreset, ref Vector2 mouseAngles);
}
