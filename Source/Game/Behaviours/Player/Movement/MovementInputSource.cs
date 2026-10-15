using FlaxEngine;
using Game.Systems.Input;

namespace Game.Behaviours.Player.Movement;

public sealed class MovementInputSource(InputManager input) : IInputSource
{
    readonly InputManager input = input;
    bool jumpQueued;

    public bool JumpPressedThisFrame { get; private set; }
    public bool SprintHeld { get; private set; }
    public Vector2 MoveAxis => new(input.GetAxis(GameAxis.MoveX), input.GetAxis(GameAxis.MoveY));

    public void Poll(bool focused)
    {
        SprintHeld = focused && input.GetAxis(GameAxis.Sprint) > 0.5f;
        JumpPressedThisFrame = jumpQueued;
        jumpQueued = false;
    }

    public void OnJumpAction() => jumpQueued = true;
}
