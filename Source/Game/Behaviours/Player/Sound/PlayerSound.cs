using System;
using FlaxEngine;
using Game.Systems.Audio;

namespace Game.Behaviours.Player;

public sealed class PlayerSound : Script
{
    [Header("References")]
    public Actor PoolHost;
    public JsonAssetReference<SoundBank> SoundBankAsset;

    [Header("Settings")]
    [Limit(0f, 1f)] public float volume = 0.8f;
    [Tooltip("Downward ray length used to detect the surface under the player.")]
    public float groundProbeDistance = 3f;
    [Tooltip("Vertical offset from the actor pivot to the feet, used as ray origin.")]
    public float feetOffset = 0.9f;
    public LayersMask WorldLayersMask = LayersMask.Default;
    [Tooltip("Minimum time between footsteps, in seconds.")]
    public float footstepCooldown = 0.1f;
    [Tooltip("Random pitch variation applied to each sound (0 = off).")]
    [Range(0, 0.5f)] public float pitchJitter = 0.05f;

    AudioSourcePool pool;
    AudioService audio;
    float lastFootstepTime = float.NegativeInfinity;

    public override void OnStart()
    {
        if (PoolHost == null)
        {
            Debug.LogWarning("PlayerSound: PoolHost not assigned, using Actor itself. " +
                             "Pooled AudioSources will be destroyed with this Actor.");
            PoolHost = Actor;
        }

        pool = new AudioSourcePool(PoolHost, initialSize: 10, maxSize: 20);
        audio = new AudioService(pool);

        if (!SoundBankAsset)  Debug.LogWarning("PlayerSound: SoundBank not assigned. No sounds will play.");
    }

    public override void OnUpdate() => audio?.Update();

    public override void OnDestroy()
    {
        audio?.StopAll();
        audio = null;
        pool = null;
    }

    public void PlayFootstep()
    {
        if (!CanPlay(out var bank)) return;

        var now = Time.GameTime;
        if (now - lastFootstepTime < footstepCooldown) return;
        lastFootstepTime = now;

        var tag = TryGetGroundTag(out var groundTag) ? groundTag : Tag.Default;
        var clip = SoundBank.FindClip(bank.FootstepSounds, tag);
        if (clip != null) PlayClipAtPosition(clip, Actor.Position);
    }

    public void PlayJump()
    {
        if (!CanPlay(out var bank) || !Actor.IsActive) return;

        var tag = TryGetGroundTag(out var groundTag) ? groundTag : Tag.Default;
        var clip = SoundBank.FindClip(bank.JumpSounds, tag);
        if (clip != null) PlayClipAtPosition(clip, Actor.Position);
    }

    public void PlayLand()
    {
        if (!CanPlay(out var bank) || !Actor.IsActive) return;

        var tag = TryGetGroundTag(out var groundTag) ? groundTag : Tag.Default;
        var clip = SoundBank.FindClip(bank.LandSounds, tag);
        if (clip != null) PlayClipAtPosition(clip, Actor.Position);
    }

    public void PlayHurt()
    {
        if (!CanPlay(out var bank) || !Actor.IsActive) return;

        var tag = TryGetGroundTag(out var groundTag) ? groundTag : Tag.Default;
        var clip = SoundBank.FindClip(bank.HurtSounds, tag);
        if (clip != null) PlayClipAtPosition(clip, Actor.Position);
    }

    bool CanPlay(out SoundBank bank)
    {
        bank = null;
        if (audio == null || !SoundBankAsset) return false;
        bank = SoundBankAsset.Instance;
        return bank != null;
    }

    bool TryGetGroundTag(out Tag tag)
    {
        tag = Tag.Default;

        var origin = Actor.Position + Vector3.Down * feetOffset;
        if (!Physics.RayCast(origin, Vector3.Down, out var hit, groundProbeDistance, WorldLayersMask, false))
            return false;

        tag = hit.Material != null ? hit.Material.Tag : Tag.Default;
        return true;
    }

    void PlayClipAtPosition(AudioClip clip, Vector3 position)
    {
        var pitch = 1f + (pitchJitter > 0f ? (Random.Shared.NextSingle() * 2f - 1f) * pitchJitter : 0f);
        audio.PlayOneShot(clip, position, volume, pitch);
    }
}