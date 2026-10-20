using System.Collections.Generic;
using FlaxEngine;

namespace Game.Systems.Audio;

public sealed class AudioSourcePool
{
    readonly Stack<AudioSource> pool = new();
    readonly Actor host;
    readonly int maxSize;
    readonly HashSet<AudioSource> active = [];

    public AudioSourcePool(Actor host, int initialSize = 5, int maxSize = 15)
    {
        this.host = host;
        this.maxSize = maxSize;
        for (var i = 0; i < initialSize; i++) pool.Push(CreateNew());
    }

    AudioSource CreateNew() => new()
    {
        Parent = host,
        PlayOnStart = false,
        IsActive = false
    };

    public AudioSource Rent()
    {
        var src = pool.TryPop(out var pooled) ? pooled : CreateNew();
        src.IsActive = true;
        active.Add(src);
        return src;
    }

    public void Return(AudioSource src)
    {
        if (src == null || !active.Remove(src)) return;

        src.Stop();
        src.Clip = null;
        src.IsActive = false;

        if (pool.Count < maxSize)
            pool.Push(src);
        else
            Object.Destroy(src);
    }

    public void Update()
    {
        List<AudioSource> finished = null;

        foreach (var src in active)
            if (src != null && !src.IsActuallyPlaying)
                (finished ??= []).Add(src);

        if (finished != null)
            foreach (var src in finished)
                Return(src);
    }
}