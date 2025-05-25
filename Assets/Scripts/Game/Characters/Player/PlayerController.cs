using UnityEngine;

public class PlayerController : EntityCtrl
{
    private PlayerStateMachine stateMachine;
    private PlayerInputHandler inputHandler;

    private float currentSpeed;
    private PlayerInventory inventory;

    [Header("Movement")]
    public float sprintMultiplier = 1.5f;
    public bool IsSprinting => Input.GetKey(KeyCode.LeftShift);
    public bool IsMoving => inputHandler != null && inputHandler.MoveInput.sqrMagnitude > 0.1f;

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

            float weight = inventory != null ? inventory.TotalWeight : 0f;

            if (weight > inventory.MaxWeight)
            {
                currentSpeed = 0f;
            }
            else if (weight > inventory.HardLimit)
            {
                currentSpeed = baseSpeed * 0.3f;
            }
            else if (weight > inventory.SoftLimit)
            {
                currentSpeed = baseSpeed * 0.6f;
            }


            // Trừ penalty từ giáp và mũ
            float armorPenalty = 0f;
            var armorMgr = GetComponent<EquippedArmorManager>();
            if (armorMgr != null)
            {
                foreach (var armor in armorMgr.GetAllEquippedArmors())
                {
                    if (armor?.armorData != null)
                    {
                        armorPenalty += armor.armorData.moveSpeedPenalty;
                    }
                }
            }

            currentSpeed = Mathf.Max(0f, currentSpeed - armorPenalty);

            // Nếu đang giữ Shift và chưa quá tải → chạy nhanh
            bool allowSprint = GetComponent<PlayerStamina>()?.IsStaminaEmpty == false;
            if (IsSprinting && weight <= inventory.MaxWeight && allowSprint)
            {
                currentSpeed *= sprintMultiplier;
            }

            transform.position += (Vector3)movement.normalized * currentSpeed * Time.deltaTime;

            Debug.Log($"🏃 Tốc độ: {currentSpeed} | Khối lượng: {weight} / {inventory?.MaxWeight}");
        }
    }
}
