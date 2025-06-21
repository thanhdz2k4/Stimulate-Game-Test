using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    private PlayerMoveState moveState;
    private PlayerIdleState idleState;
    [SerializeField] bool isMoving = false;

    [SerializeField, ReadOnly(true)] StateBase currentState;
    [SerializeField, ReadOnly(true)] StateBase previousState;



    PlayerParameters playerParameters;
    StateMachine stateMachine; private void Awake()
    {
        // Create StateMachine as regular class
        stateMachine = new StateMachine();
        moveState = new PlayerMoveState();
        idleState = new PlayerIdleState();
        playerParameters = new PlayerParameters(animator, this.gameObject);
        moveState.Initialize(playerParameters);
        idleState.Initialize(playerParameters);

        stateMachine.OnStateChanged += (previous, current) =>
        {
            previousState = previous;
            currentState = current;
        };

    }
    void Start()
    {
        stateMachine.ChangeState(idleState);
    }    void Update()
    {
        stateMachine.Update();

        // Only W/S keys trigger movement (forward/backward)
        // A/D keys are used for steering within the movement state
        isMoving = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S);

        if (isMoving && !stateMachine.IsFinishedState)
        {
            stateMachine.ChangeState(moveState);
        }
        else if (!isMoving && !stateMachine.IsFinishedState)
        {
            stateMachine.ChangeState(idleState);        }
    }

    void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    void LateUpdate()
    {
        stateMachine.LateUpdate();
    }

}

public class PlayerParameters
{
    private Animator animator;
    private GameObject playerObject;
    public Animator Animator => animator;
    public GameObject PlayerObject => playerObject;

    public PlayerParameters(Animator animator, GameObject playerObject = null)
    {
        this.animator = animator;
        this.playerObject = playerObject;
    }

}
