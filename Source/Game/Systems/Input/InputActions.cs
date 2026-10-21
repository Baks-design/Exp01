namespace Game.Systems.Input;

public readonly record struct InputActions(string Name)
{
    //UI
    public static readonly InputActions Menu = new("Menu");
    //Controller
    public static readonly InputActions FirstView = new("FirstView");
    public static readonly InputActions ThirdView = new("ThirdView");
    //Movement
    public static readonly InputActions Sprint = new("Sprint");
    public static readonly InputActions Jump = new("Jump");

    public override string ToString() => Name;
}
