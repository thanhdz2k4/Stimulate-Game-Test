
using System;
using System.Collections.Generic;
using Chef.TThanh;
using Patterns.StateMachine.TThanh;
using UnityEngine;

public class ChefController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator _animator;

    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _rotationSpeed = 720f;

    [Header("Debug")]
    [SerializeField] private ChefAction _currentAction;
    [SerializeField] private bool _isMoving = false;
    [SerializeField] private Vector2 _moveInput;
    private StateMachin<ChefAction> _stateMachine;

    // Action Configurations
    private Dictionary<ChefAction, ActionConfig> _actionConfigs = new Dictionary<ChefAction, ActionConfig>
    {
        { ChefAction.Idle, new ActionConfig("Idle", "IsIdle", 0f, true, true, false) },
        { ChefAction.Walking, new ActionConfig("Walk", "IsWalking", 0f, true, true, false) },
        { ChefAction.Cooking, new ActionConfig("Cook", "IsCooking", 2f, false, false, true) },
        { ChefAction.Cleaning, new ActionConfig("Clean", "IsCleaning", 1.5f, false, false, true) },
        { ChefAction.Serving, new ActionConfig("Serve", "IsServing", 1f, false, false, true) },
        { ChefAction.Chopping, new ActionConfig("Chop", "IsChopping", 1.2f, false, false, true) },
        { ChefAction.Washing, new ActionConfig("Wash", "IsWashing", 1.5f, false, false, true) },
        { ChefAction.Carrying, new ActionConfig("Carry", "IsCarrying", 0f, true, true, true) },
        { ChefAction.Plating, new ActionConfig("Plate", "IsPlating", 1.5f, false, false, true) },
        { ChefAction.Grilling, new ActionConfig("Grill", "IsGrilling", 2f, false, false, true) }
    };


    // Events
    public event Action<ChefAction> OnActionStarted;
    public event Action<ChefAction> OnActionCompleted;

    // Properties
    public Animator Animator => _animator;
    public float MoveSpeed => _moveSpeed;
    public float RotationSpeed => _rotationSpeed;
    public bool IsMoving => _isMoving;
    public Vector2 MoveInput => _moveInput;

    void Awake()
    {
        InitializeStateMachine();
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
            ChefAction.Cooking => new CleaningState(this, config),

        };
    }

    void Start()
    {
        // Start with Idle state
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
            if (Input.GetKeyDown(KeyCode.C)) TryStartAction(ChefAction.Cooking);
            if (Input.GetKeyDown(KeyCode.V)) TryStartAction(ChefAction.Cleaning);
            if (Input.GetKeyDown(KeyCode.E)) TryStartAction(ChefAction.Serving);
            if (Input.GetKeyDown(KeyCode.X)) TryStartAction(ChefAction.Chopping);
            if (Input.GetKeyDown(KeyCode.Z)) TryStartAction(ChefAction.Washing);
            if (Input.GetKeyDown(KeyCode.F)) TryStartAction(ChefAction.Carrying);
            if (Input.GetKeyDown(KeyCode.P)) TryStartAction(ChefAction.Plating);
            if (Input.GetKeyDown(KeyCode.G)) TryStartAction(ChefAction.Grilling);
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
        
        Debug.Log($"Chef action: {previous} → {current}");
    }
    
    // Public query methods
    public bool IsPerformingAction(ChefAction action) => _stateMachine.CurrentStateType.Equals(action);
    public bool CanMove() => _actionConfigs[_stateMachine.CurrentStateType].canMove;
    public bool CanInterrupt() => _actionConfigs[_stateMachine.CurrentStateType].canInterrupt;
    public float GetActionProgress() => (_stateMachine.GetCurrentState() as ChefActionState)?.GetProgress() ?? 0f;

}
