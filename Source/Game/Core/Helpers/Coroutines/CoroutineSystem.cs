using System;
using System.Collections;
using System.Collections.Generic;
using FlaxEngine;

namespace Game.Core.Helpers.Coroutines;

public sealed class CoroutineSystem : GamePlugin
{
    readonly List<CoroutineHandle> active = [];
    readonly Coroutine facade = new();

    public bool HasCoroutines => active.Count > 0;

    public CoroutineSystem() => _description = new PluginDescription
    {
        Name = "Coroutine System",
        Description = "Global coroutine service.",
        Category = "Tools"
    };

    public override void Initialize()
    {
        base.Initialize();
        facade.Initialize(this);
        Scripting.Update += Update;
    }

    public override void Deinitialize()
    {
        Scripting.Update -= Update;
        StopAllCoroutines();
        facade.DeInitialize();
        base.Deinitialize();
    }

    void Update()
    {
        Profiler.BeginEvent("Update Coroutines");
        try
        {
            for (var i = active.Count - 1; i >= 0; i--)
            {
                var handle = active[i];
                if (IsOwnerGone(handle.Owner))
                {
                    RemoveAt(i);
                    continue;
                }

                var current = handle.Coroutine.Current;
                if (current is null)
                {
                    Advance(ref handle, i);
                    continue;
                }

                if (current is IYieldInstruction instruction)
                {
                    if (!instruction.Step(Time.DeltaTime)) continue;

                    Advance(ref handle, i);
                    continue;
                }

                Advance(ref handle, i);
            }
        }
        finally { Profiler.EndEvent(); }
    }

    void Advance(ref CoroutineHandle handle, int index)
    {
        if (handle.Coroutine.MoveNext())
            active[index] = handle;
        else
            RemoveAt(index);
    }

    void RemoveAt(int index)
    {
        active[index].Dispose();
        active.RemoveAt(index);
    }

    static bool IsOwnerGone(object owner)
    {
        if (owner is null) return true;
        if (owner is FlaxEngine.Object flaxObj) return flaxObj == null;
        return false;
    }

    public IEnumerator Invoke(Action function, float seconds, object owner, bool timeScaleIndependent = false)
    {
        ArgumentNullException.ThrowIfNull(function);
        var routine = InvokeRoutine(function, seconds, timeScaleIndependent);
        StartCoroutine(routine, owner);
        return routine;
    }

    static IEnumerator InvokeRoutine(Action function, float seconds, bool unscaled)
    {
        yield return new WaitForSeconds(seconds, unscaled);
        function();
    }

    public void StartCoroutine(IEnumerator coroutine, object owner)
    {
        ArgumentNullException.ThrowIfNull(coroutine);
        ArgumentNullException.ThrowIfNull(owner);

        var handle = new CoroutineHandle(coroutine, owner);

        if (!coroutine.MoveNext())
        {
            (coroutine as IDisposable)?.Dispose();
            return;
        }

        active.Add(handle);
    }

    public void StopCoroutine(IEnumerator coroutine)
    {
        ArgumentNullException.ThrowIfNull(coroutine);

        for (var i = active.Count - 1; i >= 0; i--)
        {
            if (ReferenceEquals(active[i].Coroutine, coroutine))
            {
                RemoveAt(i);
                return;
            }
        }
    }

    public void StopAllCoroutines()
    {
        for (var i = active.Count - 1; i >= 0; i--) active[i].Dispose();
        active.Clear();
    }

    public void StopAllObjectCoroutines(object owner)
    {
        for (var i = active.Count - 1; i >= 0; i--)
            if (ReferenceEquals(active[i].Owner, owner))
                RemoveAt(i);
    }
}