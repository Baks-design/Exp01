using FlaxEngine;

namespace Game.Systems.Audio;

// Use:
//  jumpPool ??= new AudioSourcePool(host, 5, 20);
//  jumpAudio ??= new AudioService(jumpPool);
public sealed class AudioService(AudioSourcePool pool) 
{
    private readonly AudioSourcePool pool = pool;

    public void PlayOneShot(AudioClip clip, Vector3 position, float volume = 1f)
    {
        if (clip == null) return;

        var src = pool.Rent();
        src.Clip = clip;
        src.Volume = volume;
        src.Position = position;
        src.IsLooping = false;
        src.Play();
    }
}
