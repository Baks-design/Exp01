using FlaxEngine;

namespace Game.Behaviours.Player;

public sealed class PlayerAnimation : Script //FIXME
{
    public CharacterController controller;
    public PlayerMovement playerMovement;
    public AnimatedModel CharacterModel;
    readonly PlayerMovementData movementData = new();
    AnimGraphParameter paramSpeed;

    public override void OnStart()
    {
        if (!controller) { Debug.LogError("PlayerAnimation: CharacterController is not assigned."); return; }
        if (!playerMovement) { Debug.LogError("PlayerAnimation: PlayerMovement is not assigned."); return; }
        if (!CharacterModel) { Debug.LogError("PlayerAnimation: AnimatedModel is not assigned."); return; }

        paramSpeed = CharacterModel.GetParameter(Parameters.Speed);
    }

    public override void OnUpdate() => UpdateAnimation();

    void UpdateAnimation() => paramSpeed?.Value = movementData.horizontalVelocity * 0.1f;
}