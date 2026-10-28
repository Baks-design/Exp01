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

    public static SoundEntryByTag FindEntry(JsonAssetReference<SoundEntryByTag>[] entries, Tag tag)
    {
        if (entries == null || entries.Length == 0) return null;

        SoundEntryByTag fallback = null;

        foreach (var reference in entries)
        {
            if (!reference) continue;
            var entry = reference.Instance;
            if (entry == null) continue;

            if (entry.SurfaceTag == tag) return entry;
            if (entry.SurfaceTag == Tag.Default) fallback = entry;
        }

        return fallback;
    }

    public static AudioClip FindClip(JsonAssetReference<SoundEntryByTag>[] entries, Tag tag)
    {
        var entry = FindEntry(entries, tag);
        return entry?.PickRandom();
    }
}