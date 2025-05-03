using UnityEngine;

public class PlayerLowerBodyAnimator : MonoBehaviour
{
    private Animator animator;
    private PlayerInputHandler inputHandler;
    private PlayerStateMachine stateMachine;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        inputHandler = GetComponentInParent<PlayerInputHandler>();
        stateMachine = GetComponentInParent<PlayerStateMachine>();
    }

    private void Update()
    {
        if (stateMachine != null && stateMachine.currentMainState == PlayerStateMachine.MainState.Alive)
        {
            Vector2 moveInput = inputHandler != null ? inputHandler.MoveInput : Vector2.zero;
            bool isMoving = moveInput.magnitude > 0.1f;
            animator.SetBool("isMoving", isMoving);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }
}
