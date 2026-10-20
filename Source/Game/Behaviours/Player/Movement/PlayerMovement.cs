using FlaxEngine;
using Game.Systems.Input;

namespace Game.Behaviours.Player;

public sealed class PlayerMovement : Script
{
    [Header("Strategies")]
    public ThirdPersonMovement ThirdPerson = new();
    public FirstPersonMovement FirstPerson = new();
    [Header("References")]
    public CharacterController Controller;
    public Camera CameraActor;
    readonly InputService inputService = new();
    readonly PlayerStateMachine stateMachine = new();
    readonly PlayerMovementData movementData = new();
    IMovementStrategy strategy;
    MovementResult result;
    PlayerVerticalMovement verticalMovement;

    public PlayerMovementPreset ActivePreset { get; private set; }
    public float SpeedRatio => result.SpeedRatio;
    public bool IsGrounded => Controller.IsGrounded;

    public override void OnStart()
    {
        if (strategy == null)
            SetMovementStrategy(ThirdPerson);
        else
            RefreshActivePreset();

        verticalMovement ??= new PlayerVerticalMovement(inputService, Controller, movementData);
    }

    public override void OnUpdate()
    {
        if (!Controller || strategy == null || ActivePreset == null || !CameraActor) return;

        verticalMovement.UpdateJumpTimers(ActivePreset);
        result = strategy.CalculateMovement(inputService, ActivePreset, Controller, movementData, CameraActor);
        UpdateStateMachine(result.State);
        verticalMovement.ApplyVerticalMovement(Controller.IsGrounded, ActivePreset);
        ApplyMovement(result.Displacement);
        ApplyRotation(result.Displacement);
    }

    public void SetMovementStrategy(IMovementStrategy newStrategy)
    {
        if (newStrategy == null)
        {
            Debug.LogError("SwitchStrategy received null.");
            return;
        }

        strategy = newStrategy;
        movementData.verticalVelocity = Vector3.Zero;
        movementData.coyoteTimer = 0f;
        movementData.jumpBufferTimer = 0f;

        RefreshActivePreset();
    }

    void RefreshActivePreset()
    {
        var reference = strategy.PresetReference;
        if (reference == null || !reference.Asset)
        {
            Debug.LogError($"{strategy.GetType().Name} has no preset reference assigned.");
            ActivePreset = null;
            return;
        }

        ActivePreset = reference.Asset.CreateInstance<PlayerMovementPreset>();
    }

    void UpdateStateMachine(CharacterMoveState state)
    {
        switch (state)
        {
            case CharacterMoveState.Idle: stateMachine.GoToState(stateMachine.Idle); break;
            case CharacterMoveState.Moving: stateMachine.GoToState(stateMachine.Moving); break;
            case CharacterMoveState.Jumping: stateMachine.GoToState(stateMachine.Jumping); break;
            case CharacterMoveState.Falling: stateMachine.GoToState(stateMachine.Falling); break;
        }
    }

    void ApplyMovement(Vector3 horizontalDisplacement)
    {
        var finalMove = horizontalDisplacement + movementData.verticalVelocity * Time.DeltaTime;
        Controller.Move(finalMove);
    }

    void ApplyRotation(Vector3 displacement)
    {
        if (displacement.LengthSquared <= 0.01f) return;

        var targetRot = Quaternion.LookRotation(displacement.Normalized, Vector3.Up);
        Controller.Orientation = Quaternion.Lerp(
            Controller.Transform.Orientation, targetRot, Time.DeltaTime * ActivePreset.RotationLerpSpeed);
    }
}