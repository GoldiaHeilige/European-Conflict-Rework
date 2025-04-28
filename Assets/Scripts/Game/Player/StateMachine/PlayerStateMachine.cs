using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public enum MainState { Alive, Dead }
    public enum AliveSubState { Idle, Move }

    public MainState currentMainState { get; private set; } = MainState.Alive;
    public AliveSubState currentAliveSubState { get; private set; } = AliveSubState.Idle;

    private PlayerInputHandler inputHandler;

    private void Awake()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
    }

    private void Update()
    {
        if (currentMainState == MainState.Alive)
        {
            Vector2 input = inputHandler != null ? inputHandler.MoveInput : Vector2.zero;

            if (input.magnitude > 0.1f)
                ChangeAliveSubState(AliveSubState.Move);
            else
                ChangeAliveSubState(AliveSubState.Idle);
        }
    }

    public void ChangeAliveSubState(AliveSubState newState)
    {
        if (currentAliveSubState == newState)
            return;

        currentAliveSubState = newState;
        Debug.Log("Sub-state changed to: " + currentAliveSubState);
    }
}
