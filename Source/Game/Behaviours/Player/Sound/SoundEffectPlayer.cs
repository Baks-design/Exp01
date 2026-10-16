using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game.Behaviours.Player.Sound;

public sealed class SoundEffectPlayer(
    AudioSource[] pool, Dictionary<SoundId, AudioClip> clipLibrary, bool stealOldestWhenFull = true) : ISoundEffectPlayer
{
    readonly AudioSource[] pool = pool ?? throw new ArgumentNullException(nameof(pool));
    readonly Dictionary<SoundId, AudioClip> clipLibrary = clipLibrary ?? throw new ArgumentNullException(nameof(clipLibrary));
    readonly bool stealOldestWhenFull = stealOldestWhenFull;

    public void PlaySound(ISoundEffectClip clip, float volume = 1)
    {
        if (!clipLibrary.TryGetValue(clip.Id, out var audioClip)) return;

        var source = GetAvailableSource();
        if (source is null) return;

        source.Clip = audioClip;
        source.Volume = volume;
        source.Play();
    }

    public void StopAll()
    {
        foreach (var source in pool) source.Stop();
    }

    AudioSource GetAvailableSource()
    {
        foreach (var source in pool) if (!source.IsActuallyPlaying) return source;
        return stealOldestWhenFull ? pool[0] : null;
    }
}