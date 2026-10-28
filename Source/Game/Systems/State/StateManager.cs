using FlaxEngine;

namespace Game.Systems.State;

public sealed class StateManager : GamePlugin
{
    public static StateManager Instance => PluginManager.GetPlugin<StateManager>();
    public StateService State { get; } = new();

    public StateManager()
    {
        _description = new PluginDescription
        {
            Name = "State Manager",
            Description = "Global game state management.",
            Category = "Gameplay",
        };
    }

    public override void Initialize()
    {
        base.Initialize();
        State.Initialize();
        Scripting.Update += OnScriptingUpdate; 
    }

    public override void Deinitialize()
    {
        Scripting.Update -= OnScriptingUpdate; 
        State.Shutdown();
        base.Deinitialize();
    }

    void OnScriptingUpdate() => State.Update(Time.DeltaTime);

    public void StartGame() => State.SetState(StateId.Loading);

    public void PauseGame() => State.SetState(StateId.Paused);
    
    public void ResumeGame() => State.SetState(StateId.InGame);

    public void BackToMenu() => State.SetState(StateId.MainMenu);
}