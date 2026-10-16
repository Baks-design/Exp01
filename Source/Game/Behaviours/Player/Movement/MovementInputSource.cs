using FlaxEngine;
using Game.Systems.Input;

namespace Game.Behaviours.Player.Movement;

public sealed class MovementInputSource(IInputService input) : IInputSource
{
    readonly IInputService input = input;
    bool jumpQueued;

    public Vector2 MoveAxis => new(input.GetAxis(GameAxis.MoveX), input.GetAxis(GameAxis.MoveY));
    public bool JumpPressedThisFrame { get; private set; }
    public bool SprintHeld { get; private set; }

    public void Poll(bool focused)
    {
        SprintHeld = input.GetAction(InputActions.Sprint) && focused;
        JumpPressedThisFrame = focused && jumpQueued;
        jumpQueued = false;
    }

    public void OnJumpAction() => jumpQueued = true;
}
