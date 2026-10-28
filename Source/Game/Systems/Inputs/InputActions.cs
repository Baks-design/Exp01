namespace Game.Systems.Inputs;

public readonly record struct InputActions(string Name)
{
    public static readonly InputActions Menu = new("Menu");
    public static readonly InputActions FirstView = new("FirstView");
    public static readonly InputActions ThirdView = new("ThirdView");
    public static readonly InputActions Sprint = new("Sprint");
    public static readonly InputActions Jump = new("Jump");
    public static readonly InputActions Aim = new("Aim");
    
    public override string ToString() => Name;
}