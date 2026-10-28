using System;
using System.Collections;

namespace Game.Core.Helpers.Coroutines;

// WaitForSeconds
// This example shows a simple way to wait 5 seconds in a coroutine method.

// readonly Coroutine coroutine = new();

// void override OnStart() Coroutine.StartCoroutine(MyCoroutine(), this);

// IEnumerator MyCoroutine()
// {
//     yield return new WaitForSeconds(5);
//     Debug.Log("This is called 5 seconds later.");
// }

// Simple Invoke
// This plugin contains a simple way to execute a method after a 
// certain amount of time using the Invoke method.Here is an example of how to use it.

// void override OnStart() => Coroutine.Invoke(MethodCalledIn5Seconds, 5, this);

// void MethodCalledIn5Seconds()=>  Debug.Log("This is called 5 seconds later.");

// Wait Until
// This example waits until a specific member variable boolean is true before continuing to execute.

// bool ThisNeedsToBeTrueToContinue = false;

// void override OnStart() => Coroutine.StartCoroutine(MyCoroutine(), this);

// IEnumerator MyCoroutine()
// {
//     yield return new WaitUntil(() => ThisNeedsToBeTrueToContinue);
//     Debug.Log("This is called after ThisNeedsToBeTrueToContinue boolean is set to true.");
// }

public sealed class Coroutine
{
    CoroutineSystem system;

    public bool HasCoroutines => system != null && system.HasCoroutines;
    CoroutineSystem System =>
       system ?? throw new ObjectDisposedException(nameof(Coroutine),
           "Coroutine facade is not attached. It was never initialized, or the owning system has been deinitialized.");

    internal void Initialize(CoroutineSystem system) => this.system = system;

    internal void DeInitialize() => system = null;

    public IEnumerator Invoke(Action fn, float seconds, object owner, bool unscaled = false)
        => System.Invoke(fn, seconds, owner, unscaled);

    public void StartCoroutine(IEnumerator routine, object owner) => System.StartCoroutine(routine, owner);

    public void StopCoroutine(IEnumerator routine) => System.StopCoroutine(routine);

    public void StopAllCoroutines() => System.StopAllCoroutines();

    public void StopAllObjectCoroutines(object owner) => System.StopAllObjectCoroutines(owner);
}