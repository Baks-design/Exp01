using System.Collections.Generic;

namespace Game.Systems.Serialization;

public interface ISaveStorage
{
    void Write(Dictionary<string, object> data);
    Dictionary<string, object> Read();
}
