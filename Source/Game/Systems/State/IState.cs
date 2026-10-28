namespace Game.Systems.State;

public interface IState
{
    StateId Id { get; }

    bool CanEnter(StateId from);
    void OnEnter();
    void OnUpdate(float dt);
    void OnExit();
}