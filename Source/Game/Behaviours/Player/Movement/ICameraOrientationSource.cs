using FlaxEngine;

namespace Game.Behaviours.Player.Movement;

public interface ICameraOrientationSource
{
    Vector3 Forward { get; }
    Vector3 Right { get; }
}
