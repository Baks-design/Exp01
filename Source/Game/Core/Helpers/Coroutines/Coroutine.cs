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

    public bool HasCoroutines => system.HasCoroutines;

    internal void Initialize(CoroutineSystem coroutineSystem) => system = coroutineSystem;

    internal void DeInitialize() => system = null;

    public IEnumerator Invoke(Action function, float seconds, object owner, bool timeScaleIndependent = false)
        => system.Invoke(function, seconds, owner, timeScaleIndependent);

    public void StartCoroutine(IEnumerator coroutine, object owner) => system.StartCoroutine(coroutine, owner);

    public void StopCoroutine(IEnumerator coroutine) => system.StopCoroutine(coroutine);

    public void StopAllCoroutines() => system.StopAllCoroutines();

    public void StopAllObjectCoroutines(object owner) => system.StopAllObjectCoroutines(owner);
}