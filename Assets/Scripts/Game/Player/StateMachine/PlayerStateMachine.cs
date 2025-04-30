using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public enum MainState { Alive, Dead }
    public MainState currentMainState { get; private set; } = MainState.Alive;

    public enum MovementState { Idle, Move }
    public enum CombatState { None, Shooting }
    public enum ActionState { None, Interact }

    public MovementState currentMovementState { get; private set; } = MovementState.Idle;
    public CombatState currentCombatState { get; private set; } = CombatState.None;
    public ActionState currentActionState { get; private set; } = ActionState.None;

    private PlayerInputHandler inputHandler;

    private void Awake()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
    }

    private void Update()
    {
        if (currentMainState != MainState.Alive) return;

        HandleMovementState();
        HandleCombatState();
    }

    private void HandleMovementState()
    {
        if (inputHandler.MoveInput.magnitude > 0.1f)
            ChangeMovementState(MovementState.Move);
        else
            ChangeMovementState(MovementState.Idle);
    }

    private void HandleCombatState()
    {
        if (inputHandler.FireInput)
            ChangeCombatState(CombatState.Shooting);
        else
            ChangeCombatState(CombatState.None);
    }

    public void ChangeMovementState(MovementState newState)
    {
        if (currentMovementState == newState) return;
        currentMovementState = newState;
        // Debug.Log($"Movement State: {newState}");
    }

    public void ChangeCombatState(CombatState newState)
    {
        if (currentCombatState == newState) return;
        currentCombatState = newState;
        // Debug.Log($"Combat State: {newState}");
    }

    public void ChangeActionState(ActionState newState)
    {
        if (currentActionState == newState) return;
        currentActionState = newState;
        // Debug.Log($"Action State: {newState}");
    }
}
