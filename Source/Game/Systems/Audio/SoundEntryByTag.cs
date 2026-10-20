using System;
using FlaxEngine;

namespace Game.Systems.Audio;

[Serializable]
public sealed class SoundEntryByTag
{
    public Tag SurfaceTag;
    public AudioClip[] Clip;
}
