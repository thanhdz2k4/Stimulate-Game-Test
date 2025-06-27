
using System;
using System.Collections.Generic;
using Chef.TThanh;
using Patterns.StateMachine.TThanh;
using UnityEngine;

public class ChefController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private ThirdPersonOrbitCamBasic _camScript; // Assuming you have a camera script for player control

    [Header("----   Movement state   ----")]
    [SerializeField] private float _rotationSpeed = 720f;
    [SerializeField] private float _turnSmoothing = 0.1f; // Smoothing factor for rotation
    [SerializeField] private Transform _camera;
    [SerializeField] private float _sprintFOV = 100f; // Field of view when sprinting
    [SerializeField] private ActionConfig _walkingConfig;

    [Header("----   Idle state   ----")]
    [SerializeField] private ActionConfig _idleConfig;

    [Header("----   Cleaning state   ----")]
    [SerializeField] private ActionConfig _cleaningConfig;

    [Header("Debug")]
    [SerializeField] private ChefAction _currentAction;
    [SerializeField] private bool _isMoving = false;
    [SerializeField] private Vector2 _moveInput;
    private StateMachin<ChefAction> _stateMachine;
    public Transform Camera => _camera;
    public Rigidbody Rigidbody => _rigidbody;
    public float TurnSmoothing => _turnSmoothing;
    private Vector3 lastDirection = Vector3.zero;

    // Action Configurations
    private Dictionary<ChefAction, ActionConfig> _actionConfigs = new Dictionary<ChefAction, ActionConfig>{};

    // Events
    public event Action<ChefAction> OnActionStarted;
    public event Action<ChefAction> OnActionCompleted;

    // Properties
    public Animator Animator => _animator;
    public float RotationSpeed => _rotationSpeed;
    public bool IsMoving => _isMoving;
    public Vector2 MoveInput => _moveInput;
    public ThirdPersonOrbitCamBasic CamScript => _camScript;
    public float SprintFOV => _sprintFOV;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        InitializeConfigs();
        InitializeStateMachine();
    }

    private void InitializeConfigs()
    {
        _actionConfigs = new Dictionary<ChefAction, ActionConfig>
        {
            { ChefAction.Idle, _idleConfig },
            { ChefAction.Walking, _walkingConfig },
            { ChefAction.Cleaning, _cleaningConfig },
        };
    }

    private void InitializeStateMachine()
    {
        _stateMachine = new StateMachin<ChefAction>();

        // create and add all states
        foreach (var action in _actionConfigs)
        {
            ChefAction actionType = action.Key;
            ActionConfig config = action.Value;

            ChefActionState state = CreateActionState(actionType, config);
            _stateMachine.AddState(actionType, state);
        }
        _stateMachine.OnStateChanged += OnStateChanged;
    }

    private ChefActionState CreateActionState(ChefAction actionType, ActionConfig config)
    {
        return actionType switch
        {
            ChefAction.Idle => new IdleState(this, config),
            ChefAction.Walking => new WalkingState(this, config),
            ChefAction.Cleaning => new CleaningState(this, config),

        };
    }

    void Start()
    {
        _stateMachine.ChangeState(ChefAction.Idle);
    }

    void Update()
    {
        HandleInput();
        _stateMachine.Update();
        _currentAction = _stateMachine.CurrentStateType;
    }

    void FixedUpdate()
    {
        _stateMachine.FixedUpdate();
    }


    public void TryStartAction(ChefAction action)
    {
        if (_stateMachine.CanChangeState())
        {
            _stateMachine.ChangeState(action);
        }
    }

    private void HandleInput()
    {
        // Movement input
        _moveInput.x = Input.GetAxis("Horizontal");
        _moveInput.y = Input.GetAxis("Vertical");
        _isMoving = _moveInput.magnitude > 0.1f;

        // Action inputs
        if (_stateMachine.CanChangeState())
        {

            if (Input.GetKeyDown(KeyCode.V)) TryStartAction(ChefAction.Cleaning);

        }
    }

    public void ForceChangeAction(ChefAction action)
    {
        _stateMachine.ChangeState(action);
    }

    private void OnStateChanged(ChefAction previous, ChefAction current)
    {
        if (previous != ChefAction.Idle) // Don't fire for initial state
        {
            OnActionCompleted?.Invoke(previous);
        }

        OnActionStarted?.Invoke(current);
    }

    // Public query methods
    public bool IsPerformingAction(ChefAction action) => _stateMachine.CurrentStateType.Equals(action);
    public bool CanMove() => _actionConfigs[_stateMachine.CurrentStateType].canMove;
    public bool CanInterrupt() => _actionConfigs[_stateMachine.CurrentStateType].canInterrupt;
    public float GetActionProgress() => (_stateMachine.GetCurrentState() as ChefActionState)?.GetProgress() ?? 0f;

    // Set the last player direction of facing.
    public void SetLastDirection(Vector3 direction)
    {
        lastDirection = direction;
    }

    // Put the player on a standing up position based on last direction faced.
    public void Repositioning()
    {
        if (lastDirection != Vector3.zero)
        {
            lastDirection.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(lastDirection);
            Quaternion newRotation = Quaternion.Slerp(_rigidbody.rotation, targetRotation, _turnSmoothing);
            _rigidbody.MoveRotation(newRotation);
        }
    }

}

public enum ChefActionAnimation
{
    Idle,
    Move,
    Cleaning,
    FastRun
}
