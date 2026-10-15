namespace Game.Behaviours.Player.Sound;

public interface ISoundEffectPlayer
{
    void PlaySound(ISoundEffectClip clip, float volume = 1f);
    void StopAll();
}
