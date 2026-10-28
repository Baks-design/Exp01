using System;
using FlaxEngine;

namespace Game.Systems.Audio;

public sealed class AudioService(AudioSourcePool pool) 
{
    readonly AudioSourcePool pool = pool ?? throw new ArgumentNullException(nameof(pool));

    public void PlayOneShot(
        AudioClip clip,
        Vector3 position,
        float volume = 1f,
        float pitch = 1f,
        float minDistance = 1f)
    {
        if (clip == null) return;

        var src = pool.Rent();
        src.Clip = clip;
        src.Volume = Mathf.Saturate(volume);
        src.Pitch = Mathf.Max(0.01f, pitch);
        src.MinDistance = minDistance;
        src.Position = position;
        src.IsLooping = false;
        src.Play();
    }

    public void Update() => pool.Update();

    public void StopAll() => pool.ReturnAll();
}