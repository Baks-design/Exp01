using System;

namespace Game.Systems.Input;

public interface IInputManager : IDisposable
{
    bool GetAction(InputActions action);
    float GetAxis(GameAxis axis, bool rawValue = true);
    void Subscribe(InputActions action, Action callback);
    void Unsubscribe(InputActions action, Action callback);
}
