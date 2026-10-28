using System;
using FlaxEngine;

namespace Game.Systems.Audio;

[Serializable]
public sealed class SoundEntryByTag
{
    public Tag SurfaceTag;
    public AudioClip[] Clips = [];

    public AudioClip PickRandom()
    {
        if (Clips == null || Clips.Length == 0) return null;
        if (Clips.Length == 1) return Clips[0];

        var start = Random.Shared.Next(Clips.Length);
        for (var i = 0; i < Clips.Length; i++)
        {
            var clip = Clips[(start + i) % Clips.Length];
            if (clip != null) return clip;
        }
        
        return null;
    }
}