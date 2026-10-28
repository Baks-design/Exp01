namespace Game.Systems.Serialization;

public sealed class MemorySaveStorage : ISaveStorage
{
    public string Payload { get; set; }

    public void Write(string json) => Payload = json;
    
    public string Read() => Payload;
}