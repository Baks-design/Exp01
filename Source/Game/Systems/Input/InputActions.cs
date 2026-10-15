namespace Game.Systems.Input;

public readonly record struct InputActions(string Name)
{
    public static readonly InputActions Jump = new("Jump");
    public static readonly InputActions Sprint = new("Sprint");
    public static readonly InputActions Interact = new("Interact");
    public static readonly InputActions Menu = new("Menu");

    public override string ToString() => Name;
}
