using System.Collections;

namespace Game.Core.Helpers.Coroutines;

// A helper class to manage a running coroutine instance, including its current instruction.
public struct CoroutineHandle(IEnumerator coroutine, object owner)
{
    public IEnumerator Coroutine = coroutine;
    public object Owner = owner;
}