using UnityEngine;

public class PlayerLowerBodyCtrl : MonoBehaviour
{
    private PlayerInputHandler inputHandler;
    private PlayerStateMachine stateMachine;

    private void Awake()
    {
        inputHandler = GetComponentInParent<PlayerInputHandler>();
        stateMachine = GetComponentInParent<PlayerStateMachine>();
    }

    private void Update()
    {
        if (stateMachine != null && stateMachine.currentMainState == PlayerStateMachine.MainState.Alive)
        {
            Vector2 moveInput = inputHandler != null ? inputHandler.MoveInput : Vector2.zero;

            if (moveInput.magnitude > 0.1f)
            {
                float angle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;
  
                transform.rotation = Quaternion.Euler(0, 0, angle + 90f);
            }
        }
    }
}
