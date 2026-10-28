namespace Game.Behaviours.Player;

public interface ICameraStrategy
{
    void Configure();
    void Execute(CameraContext ctx);
}