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
    private readonly InputService inputService = new();
    private readonly PlayerStateMachine stateMachine = new();
    private readonly PlayerMovementData movementData = new();
    private IMovementStrategy strategy;
    private MovementResult result;
    private PlayerAxisMovement axisMovement;

    public PlayerMovementPreset ActivePreset { get; private set; }
    public float SpeedRatio => result.SpeedRatio;
    public bool IsGrounded => Controller.IsGrounded;

    public override void OnStart()
    {
        if (strategy == null)
            SetMovementStrategy(ThirdPerson);
        else
            RefreshActivePreset();

        axisMovement ??= new PlayerAxisMovement(inputService, Controller, movementData);
    }

    public override void OnUpdate()
    {
        if (!Controller || strategy == null || ActivePreset == null || !CameraActor) return;

        axisMovement.UpdateJumpTimers(ActivePreset);
        result = strategy.CalculateMovement(inputService, Controller, CameraActor, ActivePreset, movementData);
        axisMovement.UpdateHorizontalVelocity(result.TargetVelocity, ActivePreset);
        UpdateStateMachine(result.State);
        axisMovement.UpdateVerticalVelocity(Controller.IsGrounded, ActivePreset);
        ApplyMovement();
        ApplyRotation();
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

    private void RefreshActivePreset()
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

    private void UpdateStateMachine(CharacterMoveState state)
    {
        switch (state)
        {
            case CharacterMoveState.Idle: stateMachine.GoToState(stateMachine.Idle); break;
            case CharacterMoveState.Moving: stateMachine.GoToState(stateMachine.Moving); break;
            case CharacterMoveState.Jumping: stateMachine.GoToState(stateMachine.Jumping); break;
            case CharacterMoveState.Falling: stateMachine.GoToState(stateMachine.Falling); break;
        }
    }

    private void ApplyMovement()
    {
        var finalMove = (movementData.horizontalVelocity + movementData.verticalVelocity) * Time.DeltaTime;
        Controller.Move(finalMove);
    }

    private void ApplyRotation()
    {
        if (movementData.horizontalVelocity.LengthSquared <= 0.01f) return;

        var targetRot = Quaternion.LookRotation(movementData.horizontalVelocity.Normalized, Vector3.Up);
        Controller.Orientation = Quaternion.Lerp(
            Controller.Transform.Orientation, targetRot, Time.DeltaTime * ActivePreset.RotationLerpSpeed);
    }
}