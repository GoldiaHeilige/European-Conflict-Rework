using UnityEngine;

public class PlayerController : EntityCtrl
{
    public SpriteRenderer weaponHolderRenderer;
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
            stateMachine.currentAliveSubState == PlayerStateMachine.AliveSubState.Move)
        {
            Vector2 movement = inputHandler != null ? inputHandler.MoveInput : Vector2.zero;
            transform.position += (Vector3)movement.normalized * moveSpeed * Time.deltaTime;
        }
    }

    public void EquipWeapon(Sprite weaponSprite, float weaponSpeedMultiplier)
    {
        if (weaponHolderRenderer != null)
            weaponHolderRenderer.sprite = weaponSprite;
        moveSpeed = baseMoveSpeed * weaponSpeedMultiplier;
    }
}