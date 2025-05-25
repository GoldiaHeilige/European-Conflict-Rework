using UnityEngine;

public class PlayerController : EntityCtrl
{
    private PlayerStateMachine stateMachine;
    private PlayerInputHandler inputHandler;

    private float currentSpeed;
    private PlayerInventory inventory;

    protected override void Awake()
    {
        base.Awake();
        stateMachine = GetComponent<PlayerStateMachine>();
        inputHandler = GetComponent<PlayerInputHandler>();

        stats = GetComponent<EntityStats>();
        inventory = GetComponent<PlayerInventory>();
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {

        if (stateMachine != null &&
            stateMachine.currentMainState == PlayerStateMachine.MainState.Alive &&
            stateMachine.currentMovementState == PlayerStateMachine.MovementState.Move)
        {
            Vector2 movement = inputHandler != null ? inputHandler.MoveInput : Vector2.zero;

            float baseSpeed = MoveSpeed; 
            float currentSpeed = baseSpeed;

            if (inventory != null)
            {
                float weight = inventory.TotalWeight;

                if (weight > inventory.MaxWeight)
                {
                    currentSpeed = 0f;
                }
                else if (weight > inventory.SoftLimit)
                {
                    currentSpeed = baseSpeed * 0.5f;
                }
            }

            transform.position += (Vector3)movement.normalized * currentSpeed * Time.deltaTime;

            Debug.Log($"🏃 Tốc độ: {currentSpeed} | Khối lượng: {inventory?.TotalWeight} / {inventory?.MaxWeight}");

        }
    }
}
