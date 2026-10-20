using System;
using FlaxEngine;

namespace Game.Systems.Audio;

[Serializable]
public sealed class SoundBank
{
    public JsonAssetReference<SoundEntryByTag>[] FootstepSounds = [];
    public JsonAssetReference<SoundEntryByTag>[] JumpSounds = [];
    public JsonAssetReference<SoundEntryByTag>[] LandSounds = [];
    public JsonAssetReference<SoundEntryByTag>[] HurtSounds = [];
}
