using System.IO;
using FlaxEngine;
using Game.Systems.Input;
using Game.Systems.Scene;
using Game.Systems.Serialization;

namespace Game.Behaviours.Player;

public sealed class PlayerController : Script
{
    public PlayerMovement movement;
    public PlayerCamera camera;
    public SceneReference sceneReference;
    readonly InputService input = new();
    SerializationService serializationService;

    public override void OnAwake()
    {
        var saveDir = Globals.ProductLocalFolder;
        if (!Directory.Exists(saveDir))  Directory.CreateDirectory(saveDir);
        var savePath = Path.Combine(Globals.ProductLocalFolder, "save.json");
        serializationService = new SerializationService(new FileSaveStorage(savePath));
    }

    public override void OnStart()
    {
        if (!movement) { Debug.LogError("PlayerController: PlayerMovement is not assigned."); return; }
        if (!camera) { Debug.LogError("PlayerController: PlayerCamera is not assigned."); return; }

        SetToFirstView();
    }

    public override void OnUpdate()
    {
        ChangeView();
        SaveGame();
        LoadGame();
        LoadScene();
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

    void LoadGame()
    {
        if (Input.Keyboard.GetKeyDown(KeyboardKeys.P))
        {
            serializationService.LoadAll();
            Debug.Log("[SerializationService] LoadAll started.");
        }
    }

    void LoadScene()
    {
        if (Input.Keyboard.GetKeyDown(KeyboardKeys.L))
        {
            SceneService.RequestTransition(new SceneTransitionRequest(sceneReference));
            Debug.Log("Load Scene.");
            serializationService.LoadAll();
            Debug.Log("[SerializationService] LoadAll started.");
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
