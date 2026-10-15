namespace Game.Systems.Input;

public readonly record struct GameAxis(string Name)
{
    public static readonly GameAxis MoveX = new("Horizontal");
    public static readonly GameAxis MoveY = new("Vertical");
    public static readonly GameAxis LookX = new("Mouse X");
    public static readonly GameAxis LookY = new("Mouse Y");
    public static readonly GameAxis Sprint = new("Sprint");

    public override string ToString() => Name;
}
