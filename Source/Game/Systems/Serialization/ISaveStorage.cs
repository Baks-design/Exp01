namespace Game.Systems.Serialization;

public interface ISaveStorage
{
    void Write(string json);
    string Read();
}