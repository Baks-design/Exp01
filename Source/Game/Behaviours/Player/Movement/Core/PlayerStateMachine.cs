using FlaxEngine.Utilities;

namespace Game.Behaviours.Player;

public sealed class PlayerStateMachine : StateMachine
{
    public readonly IdleState Idle;
    public readonly MovingState Moving;
    public readonly JumpingState Jumping;
    public readonly FallingState Falling;

    public PlayerStateMachine()
    {
        AddState(Idle = new IdleState());
        AddState(Moving = new MovingState());
        AddState(Jumping = new JumpingState());
        AddState(Falling = new FallingState());

        GoToState(Idle);
    }
}
