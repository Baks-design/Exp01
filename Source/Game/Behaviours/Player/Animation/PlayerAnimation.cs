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
        if (!controller)
        {
            Debug.LogError("PlayerAnimation: CharacterController is not assigned.");
            return;
        }
        if (!playerMovement)
        {
            Debug.LogError("PlayerAnimation: PlayerMovement is not assigned.");
            return;
        }
        if (!animatedModel)
        {
            Debug.LogError("PlayerAnimation: AnimatedModel is not assigned.");
            return;
        }

        paramSpeed = animatedModel.GetParameter("Speed");
        if (paramSpeed == null) Debug.LogWarning("PlayerAnimation: Anim Graph parameter 'Speed' not found.");
    }

    public override void OnUpdate() => UpdateAnimation();

    void UpdateAnimation()
    {
        if (!controller || !animatedModel) return;

        var velocity = controller.Velocity;
        var horizontalSpeed = new Vector3(velocity.X, 0, velocity.Z).Length;

        paramSpeed?.Value = horizontalSpeed;
    }
}