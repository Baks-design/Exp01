using System.Collections.Generic;
using FlaxEngine;

namespace Game.Behaviours.Player.Sound;

public sealed class PlayerSoundController : Script
{
    public int PoolSize = 10;
    public required AudioClip JumpClip;
    public required AudioClip HurtClip;
    public required AudioClip LandClip;
    public required AudioClip FootstepClip;
    static readonly SoundEffectClip JumpSound = new(SoundId.Jump);
    static readonly SoundEffectClip HurtSound = new(SoundId.Hurt);
    static readonly SoundEffectClip LandSound = new(SoundId.Land);
    SoundEffectPlayer soundEffectPlayer;

    public override void OnStart()
    {
        var library = new Dictionary<SoundId, AudioClip>
        {
            [SoundId.Jump] = JumpClip,
            [SoundId.Hurt] = HurtClip,
            [SoundId.Land] = LandClip,
        };
        if (FootstepClip is not null) library[SoundId.Footstep] = FootstepClip;

        var pool = new AudioSource[PoolSize];
        for (var i = 0; i < PoolSize; i++)
        {
            var src = Actor.AddChild<AudioSource>();
            src.Name = $"SFX_{i}";
            src.PlayOnStart = false;
            pool[i] = src;
        }

        soundEffectPlayer ??= new SoundEffectPlayer(pool, library);
    }

    public void RaiseJump() => soundEffectPlayer?.PlaySound(JumpSound, 0.8f);
    public void RaiseHurt() => soundEffectPlayer.PlaySound(HurtSound, 1f);
    public void RaiseLand() => soundEffectPlayer.PlaySound(LandSound, 0.6f);
}