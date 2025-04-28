using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
/*    public bool FireInput { get; private set; }*/

    private PlayerInput controls;

    private void Awake()
    {
        controls = new PlayerInput();
        controls.Gameplay.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => MoveInput = Vector2.zero;
/*        controls.Gameplay.Fire.performed += ctx => FireInput = true;
        controls.Gameplay.Fire.canceled += ctx => FireInput = false;*/
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();
}