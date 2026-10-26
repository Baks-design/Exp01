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

public class Coroutine
{
    CoroutineSystem system;

    internal void Initialize(CoroutineSystem coroutineSystem) => system = coroutineSystem;

    internal void DeInitialize() => system = null;

    /// <summary>
    /// Invokes a function after a specified number of seconds
    /// </summary>
    /// <param name="function">The function to invoke.</param>
    /// <param name="seconds">The number of seconds to wait.</param>
    /// <param name="owner">The owner of this invocation.</param>
    /// <param name="timeScaleIndependent">Whether to use the time scale or not.</param>
    /// <returns>The coroutine that is handling this invocation.</returns>
    public IEnumerator Invoke(Action function, float seconds, object owner, bool timeScaleIndependent = false)
        => system.Invoke(function, seconds, owner, timeScaleIndependent);

    /// <summary>
    /// Starts a new coroutine.
    /// </summary>
    /// <param name="coroutine">The IEnumerator to start.</param>
    /// <param name="owner">The owner of the coroutine. Generally a script or an actor.</param>
    public void StartCoroutine(IEnumerator coroutine, object owner)
        => system.StartCoroutine(coroutine, owner);

    /// <summary>
    /// Stops the given coroutine.
    /// </summary>
    /// <param name="coroutine">The coroutine to stop.</param>
    public void StopCoroutine(IEnumerator coroutine) => system.StopCoroutine(coroutine);

    /// <summary>
    /// Stops all active coroutines.
    /// </summary>
    public void StopAllCoroutines() => system.StopAllCoroutines();

    /// <summary>
    /// Removes all coroutines owned by the given object.
    /// </summary>
    /// <param name="owner">The owning object.</param>
    public void StopAllObjectCoroutines(object owner) => system.StopAllObjectCoroutines(owner);

    /// <summary>
    /// Whether there are any active coroutines.
    /// </summary>
    public bool HasCoroutines() => system.HasCoroutines();
}