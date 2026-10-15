using FlaxEngine;

namespace Game.Behaviours.Player.Movement;

public interface IInputSource
{
    Vector2 MoveAxis { get; }
    bool JumpPressedThisFrame { get; }
    bool SprintHeld { get; }
}
