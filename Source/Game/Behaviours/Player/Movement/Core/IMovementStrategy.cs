namespace Game.Behaviours.Player;

public interface IMovementStrategy
{
    void Execute(MovementContext ctx, float deltaTime);
}