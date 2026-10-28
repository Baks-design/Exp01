using FlaxEngine;

namespace Game.Systems.State;

public abstract class StateBase : IState
{
    public abstract StateId Id { get; }

    public abstract bool CanEnter(StateId from);
    public virtual void OnEnter() => Debug.Log($"[State] Enter {Id}");
    public virtual void OnUpdate(float dt) { }
    public virtual void OnExit() => Debug.Log($"[State] Exit {Id}");
}
