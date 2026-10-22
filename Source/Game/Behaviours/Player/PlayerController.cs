using FlaxEngine;
using Game.Systems.Input;

namespace Game.Behaviours.Player;

public sealed class PlayerController : Script
{
    public PlayerMovement movement;
    public PlayerCamera camera;
    readonly InputService input = new();

    public override void OnStart()
    {
        if (!movement)
        {
            Debug.LogError("PlayerController: PlayerMovement is not assigned.");
            return;
        }
        if (!camera)
        {
            Debug.LogError("PlayerController: PlayerCamera is not assigned.");
            return;
        }

        SetToFirstView();
    }

    public override void OnUpdate() => ChangeStrategies();

    void ChangeStrategies()
    {
        if (input.GetAction(InputActions.FirstView))
            SetToFirstView();
        else if (input.GetAction(InputActions.ThirdView))
            SetToThirdView();
    }

    void SetToFirstView()
    {
        movement.SetMovementStrategy(movement.firstPerson);
        camera.SetCameraStrategy(camera.firstPerson);
        Debug.Log("Enter in First View");
    }

    void SetToThirdView()
    {
        movement.SetMovementStrategy(movement.thirdPerson);
        camera.SetCameraStrategy(camera.thirdPerson);
        Debug.Log("Enter in Third View");
    }

    public void SetMovementAndRotation(
        float horizontal, float vertical, float pitch, float yaw)
    {
        movement.SetMovement(horizontal, vertical);
        camera.SetRotation(pitch, yaw);
        Debug.Log("Set Movement And/Or Rotation");
    }
}
