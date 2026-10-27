using System;
using System.Collections;
using System.Collections.Generic;
using FlaxEngine;

namespace Game.Core.Helpers.Coroutines;

public sealed class CoroutineSystem : GamePlugin
{
    readonly List<CoroutineHandle> activeCoroutines = [];
    readonly Coroutine coroutine = new();

    public bool HasCoroutines => activeCoroutines.Count > 0;

    public override void Initialize()
    {
        base.Initialize();
        coroutine.Initialize(this);
        Scripting.Update += Update;
    }

    void Update()
    {
        Profiler.BeginEvent("Update Coroutines");
        for (var i = activeCoroutines.Count - 1; i >= 0; i--)
        {
            var instance = activeCoroutines[i];
            if (instance.Owner == null)
            {
                activeCoroutines.RemoveAt(i);
                continue;
            }

            var currentElement = instance.Coroutine.Current;
            if (currentElement is IYieldInstruction instruction)
            {
                if (!instruction.Step(Time.DeltaTime)) continue;
                if (!instance.Coroutine.MoveNext()) activeCoroutines.RemoveAt(i);
            }
            else if (currentElement == null)
            {
                if (!instance.Coroutine.MoveNext())
                {
                    activeCoroutines.RemoveAt(i);
                    continue;
                }
            }
        }
        Profiler.EndEvent();
    }

    public override void Deinitialize()
    {
        StopAllCoroutines();
        Scripting.Update -= Update;
        coroutine.DeInitialize();
        base.Deinitialize();
    }

    public IEnumerator Invoke(Action function, float seconds, object owner, bool timeScaleIndependent = false)
    {
        var routine = InvokeCoroutine(function, seconds, timeScaleIndependent);
        StartCoroutine(routine, owner);
        return routine;
    }

    static IEnumerator InvokeCoroutine(Action function, float seconds, bool timeScaleIndependent = false)
    {
        yield return new WaitForSeconds(seconds, timeScaleIndependent);
        function();
    }

    public void StartCoroutine(IEnumerator coroutine, object owner)
    {
        var handle = new CoroutineHandle(coroutine, owner);
        handle.Coroutine.MoveNext();
        activeCoroutines.Add(handle);
    }

    public void StopCoroutine(IEnumerator coroutine)
    {
        for (var i = activeCoroutines.Count - 1; i >= 0; i--)
        {
            if (Equals(activeCoroutines[i].Coroutine, coroutine))
            {
                activeCoroutines.RemoveAt(i);
                return;
            }
        }
    }

    public void StopAllCoroutines() => activeCoroutines.Clear();

    public void StopAllObjectCoroutines(object owner)
    {
        for (var i = activeCoroutines.Count - 1; i >= 0; i--)
            if (Equals(activeCoroutines[i].Owner, owner))
                activeCoroutines.RemoveAt(i);
    }
}