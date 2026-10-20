using FlaxEngine;

namespace Game.Behaviours.Player;

public sealed class PlayerAnimation : Script
{
    public PlayerMovement playerMovement;
    public AnimatedModel CharacterModel;
    private AnimGraphParameter paramSpeed;
    private AnimGraphParameter paramIsGrounded;

    public override void OnStart()
    {
        if (CharacterModel)
        {
            paramSpeed = CharacterModel.GetParameter(Parameters.Speed);
            paramIsGrounded = CharacterModel.GetParameter(Parameters.IsGrounded);
        }
    }

    public override void OnUpdate() => UpdateAnimation();

    private void UpdateAnimation()
    {
        if (!CharacterModel) return;

        paramSpeed?.Value = playerMovement.SpeedRatio;
        paramIsGrounded?.Value = playerMovement.IsGrounded ? 1f : 0f;
    }
}