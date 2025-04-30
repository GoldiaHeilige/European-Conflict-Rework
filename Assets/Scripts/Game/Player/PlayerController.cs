using UnityEngine;

public class PlayerController : EntityCtrl
{
    private PlayerStateMachine stateMachine;
    private PlayerInputHandler inputHandler;

    protected override void Awake()
    {
        base.Awake();
        stateMachine = GetComponent<PlayerStateMachine>();
        inputHandler = GetComponent<PlayerInputHandler>();
    }

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        if (stateMachine != null &&
            stateMachine.currentMainState == PlayerStateMachine.MainState.Alive &&
            stateMachine.currentMovementState == PlayerStateMachine.MovementState.Move)
        {
            Vector2 movement = inputHandler != null ? inputHandler.MoveInput : Vector2.zero;
            transform.position += (Vector3)movement.normalized * moveSpeed * Time.deltaTime;
        }
    }
}
