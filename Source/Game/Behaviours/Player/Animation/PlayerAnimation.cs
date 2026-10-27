using FlaxEngine;

namespace Game.Behaviours.Player;

public sealed class PlayerAnimation : Script
{
    public CharacterController controller;
    public PlayerMovement playerMovement;
    public AnimatedModel animatedModel;
    AnimGraphParameter paramSpeed;

    public override void OnStart()
    {
        if (!controller) { Debug.LogError("PlayerAnimation: CharacterController is not assigned."); return; }
        if (!playerMovement) { Debug.LogError("PlayerAnimation: PlayerMovement is not assigned."); return; }
        if (!animatedModel) { Debug.LogError("PlayerAnimation: AnimatedModel is not assigned."); return; }

        paramSpeed = animatedModel?.GetParameter(Parameters.Speed);
    }

    public override void OnUpdate() => UpdateAnimation();

    void UpdateAnimation() => paramSpeed?.Value = controller.Velocity.LengthSquared;
}