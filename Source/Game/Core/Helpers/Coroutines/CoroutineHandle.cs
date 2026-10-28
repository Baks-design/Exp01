using System;
using System.Collections;

namespace Game.Core.Helpers.Coroutines;

public struct CoroutineHandle(IEnumerator coroutine, object owner)
{
    public IEnumerator Coroutine = coroutine;
    public object Owner = owner;

    public void Dispose()
    {
        (Coroutine as IDisposable)?.Dispose();
        Coroutine = null!;
        Owner = null!;
    }
}