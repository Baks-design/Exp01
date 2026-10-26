using FlaxEngine;
using Game.Core.Cursor;
using Game.Systems.Inputs;
using Game.Systems.Serialization;
using Game.Systems.State;

namespace Game.Behaviours.Player;

public sealed class PlayerController : Script
{
    public PlayerMovement movement;
    public PlayerCamera camera;
    public SceneReference sceneReference;
    readonly InputService input = new();
    readonly SerializationService serializationService;
    readonly CursorService cursorService = new();
    readonly StateService StateService = new();

    public override void OnAwake()
    {
        StateService.Initialize();
        StateService.SetState(StateId.InGame);
        if (StateService.CurrentId != StateId.InGame)
            Debug.LogError($"[Bootstrap] Failed to enter {StateId.InGame} state.");
    }

    public override void OnStart()
    {
        if (!movement) { Debug.LogError("PlayerController: PlayerMovement is not assigned."); return; }
        if (!camera) { Debug.LogError("PlayerController: PlayerCamera is not assigned."); return; }
    }

    public override void OnEnable() => SetToFirstView();

    public override void OnUpdate()
    {
        cursorService.Apply(CursorState.Gameplay);

        ChangeView();
        SaveGame();
    }

    void ChangeView()
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

    void SaveGame()
    {
        if (Input.Keyboard.GetKeyDown(KeyboardKeys.O))
        {
            serializationService.SaveAll();
            Debug.Log($"[SerializationService] SaveAll started.");
        }
    }

    public void SetMovementAndLook(
        float horizontal, float vertical, float pitch, float yaw)
    {
        movement.SetMovement(horizontal, vertical);
        camera.SetRotation(pitch, yaw);
        Debug.Log("Set Movement And/Or Rotation");
    }
}
