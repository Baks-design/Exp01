namespace Game.Systems.Serialization;

public interface ISaveable
{
    string SaveKey { get; }

    string CaptureState();
    void RestoreState(string state);
}