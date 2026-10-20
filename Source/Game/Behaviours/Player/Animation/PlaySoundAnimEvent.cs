using FlaxEngine;

namespace Game.Behaviours.Player;

public sealed class PlaySoundAnimEvent : AnimEvent
{
    public PlayerSound playerSound;

    public override void OnEvent(AnimatedModel actor, Animation anim, float time, float deltaTime)
    {
        var system = playerSound ?? actor.GetScript<PlayerSound>();
        system?.PlayFootstep();
    }
}
