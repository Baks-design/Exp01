using System;
using FlaxEngine;
using Game.Systems.Audio;

namespace Game.Behaviours.Player;

public sealed class PlayerSound : Script
{
    [Header("Settings")]
    public float volume = 0.8f;
    public float maxDistance = 10f;
    public LayersMask WorldLayersMask = LayersMask.Default;
    [Header("References")]
    public Actor PoolHost;
    public JsonAssetReference<SoundBank> SoundBank;
    private AudioSourcePool pool;

    public override void OnStart()
    {
        if (PoolHost == null)
        {
            Debug.LogWarning("PlayerSound: PoolHost not assigned, using Actor itself. " +
                             "Pooled AudioSources will be destroyed with this Actor.");
            PoolHost = Actor;
        }
        pool ??= new AudioSourcePool(PoolHost, 10, 20);

        if (!SoundBank) Debug.LogWarning("PlayerSound: SoundBank not assigned. No sounds will play.");
    }

    public override void OnUpdate() => pool.Update();

    public void PlayFootstep()
    {
        if (!CanPlay(out var bank) || !TryGetGroundTag(out var tag)) return;

        var clip = FindClip(bank.FootstepSounds, tag);
        if (clip == null) return;

        PlayClipAtPosition(clip, Actor.Position);
    }

    public void PlayJump()
    {
        if (!CanPlay(out var bank) || !Actor.IsActive || !TryGetGroundTag(out var tag)) return;

        var clip = FindClip(bank.JumpSounds, tag);
        if (clip == null) return;

        PlayClipAtPosition(clip, Actor.Position);
    }

    public void PlayLand()
    {
        if (!CanPlay(out var bank) || !Actor.IsActive || !TryGetGroundTag(out var tag)) return;

        var clip = FindClip(bank.LandSounds, tag);
        if (clip == null) return;

        PlayClipAtPosition(clip, Actor.Position);
    }

    public void PlayHurt()
    {
        if (!CanPlay(out var bank) || !Actor.IsActive || !TryGetGroundTag(out var tag)) return;

        var clip = FindClip(bank.HurtSounds, tag);
        if (clip == null) return;

        PlayClipAtPosition(clip, Actor.Position);
    }

    private bool CanPlay(out SoundBank bank)
    {
        bank = null;
        if (pool == null || !SoundBank) return false;
        bank = SoundBank.Instance;
        return bank != null;
    }

    private bool TryGetGroundTag(out Tag tag)
    {
        tag = Tag.Default;
        var feetPos = Actor.Position;
        if (!Physics.RayCast(feetPos, Vector3.Down, out var hit, maxDistance, WorldLayersMask)) return false;
        tag = hit.Material != null ? hit.Material.Tag : Tag.Default;
        return true;
    }

    private static AudioClip FindClip(JsonAssetReference<SoundEntryByTag>[] entries, Tag tag)
    {
        if (entries == null || entries.Length == 0) return null;

        var entry = FindEntry(entries, tag);
        return entry != null ? PickRandomClip(entry.Clip) : null;
    }

    private static SoundEntryByTag FindEntry(JsonAssetReference<SoundEntryByTag>[] entries, Tag tag)
    {
        foreach (var reference in entries)
        {
            if (!reference) continue;

            var entry = reference.Instance;
            if (entry == null) continue;
            if (entry.SurfaceTag == tag) return entry;
        }

        foreach (var reference in entries)
        {
            if (!reference) continue;

            var entry = reference.Instance;
            if (entry == null) continue;
            if (entry.SurfaceTag == Tag.Default) return entry;
        }

        return null;
    }

    private static AudioClip PickRandomClip(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0) return null;

        if (clips.Length == 1) return clips[0];

        var start = (int)(DateTime.Now.Ticks % clips.Length);
        for (var i = 0; i < clips.Length; i++)
        {
            var clip = clips[(start + i) % clips.Length];
            if (clip != null) return clip;
        }

        return null;
    }

    private void PlayClipAtPosition(AudioClip clip, Vector3 position)
    {
        if (clip == null || pool == null) return;

        var src = pool.Rent();
        src.Position = position;
        src.Clip = clip;
        src.Volume = Mathf.Saturate(volume);
        src.IsLooping = false;
        src.Play();
    }
}