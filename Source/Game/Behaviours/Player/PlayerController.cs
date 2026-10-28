using FlaxEngine;
using Game.Systems.Bootstrap;
using Game.Systems.Inputs;

namespace Game.Behaviours.Player;

public sealed class PlayerController : Script
{
    public PlayerMovement movement;
    public PlayerCamera camera;
    public SceneReference sceneReference;
    InputService inputService;

    public override void OnStart()
    {
        if (!movement) { Debug.LogError("PlayerController: PlayerMovement is not assigned."); return; }
        if (!camera) { Debug.LogError("PlayerController: PlayerCamera is not assigned."); return; }

        GameBootstrap.Services.TryResolve(out inputService);
    }

    public override void OnEnable() => SetToThirdView();

    public override void OnUpdate() => ChangeView();

    void ChangeView()
    {
        if (inputService.GetAction(InputActions.FirstView)) SetToFirstView();
        else if (inputService.GetAction(InputActions.ThirdView)) SetToThirdView();
    }

    void SetToFirstView()
    {
        movement.SetMovementStrategy(movement.firstPerson);
        camera.SetCameraStrategy(camera.firstPerson);
        Debug.Log("Enter in First View");
    }

    void SetToThirdView()
    {
        movement.SetMovementStrategy(movement.topDown);
        camera.SetCameraStrategy(camera.topDown);
        Debug.Log("Enter in Third View");
    }

    public void SetMovementAndLook(float horizontal, float vertical, float pitch, float yaw)
    {
        movement.AddImpulse(new Vector3(horizontal, 0f, vertical));
        camera.SetRotation(pitch, yaw);
        Debug.Log("Set Movement And/Or Rotation");
    }
}
