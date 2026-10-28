using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game.Systems.Audio;

public sealed class AudioSourcePool
{
    readonly Stack<AudioSource> pool = new();
    readonly HashSet<AudioSource> active = [];
    readonly List<AudioSource> finishedBuffer = [];
    readonly Actor host;
    readonly int maxSize;

    public int ActiveCount => active.Count;
    public int IdleCount => pool.Count;

    public AudioSourcePool(Actor host, int initialSize = 5, int maxSize = 15)
    {
        this.host = host ?? throw new ArgumentNullException(nameof(host));
        this.maxSize = Mathf.Max(1, maxSize);

        var clampedInitial = Mathf.Clamp(initialSize, 0, this.maxSize);
        for (var i = 0; i < clampedInitial; i++) pool.Push(CreateNew());
    }

    AudioSource CreateNew() => new()
    {
        Parent = host,
        PlayOnStart = false,
        IsActive = false,
        MinDistance = 1f,
        Attenuation = 1f,
        Volume = 1f,
        Pitch = 1f,
        IsLooping = false
    };

    public AudioSource Rent()
    {
        var src = pool.TryPop(out var pooled) ? pooled : CreateNew();
        src.Clip = null;
        src.Volume = 1f;
        src.Pitch = 1f;
        src.IsLooping = false;
        src.IsActive = true;
        active.Add(src);
        return src;
    }

    public void Return(AudioSource src)
    {
        if (src == null || !active.Remove(src)) return;

        src.Stop();
        src.Clip = null;
        src.IsLooping = false;
        src.IsActive = false;

        if (pool.Count < maxSize)
            pool.Push(src);
        else
            FlaxEngine.Object.Destroy(src);
    }

    public void ReturnAll()
    {
        if (active.Count == 0) return;

        finishedBuffer.Clear();
        finishedBuffer.AddRange(active);
        foreach (var src in finishedBuffer) Return(src);
        finishedBuffer.Clear();
    }

    public void Update()
    {
        if (active.Count == 0) return;

        finishedBuffer.Clear();

        foreach (var src in active) 
            if (src == null || !src.IsActuallyPlaying) 
                finishedBuffer.Add(src);

        if (finishedBuffer.Count == 0) return;

        foreach (var src in finishedBuffer) Return(src);

        finishedBuffer.Clear();
    }
}