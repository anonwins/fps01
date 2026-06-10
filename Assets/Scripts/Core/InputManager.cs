using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    private Vector2 moveInput = Vector2.zero;
    public Vector2 MoveInput => moveInput;
    private Vector2 lookInput = Vector2.zero;
    public Vector2 LookInput => lookInput;
    public bool RunHeld { get; private set; }

    public event System.Action<Vector2> OnMoveInput;
    public event System.Action<Vector2> OnLookInput;
    public event System.Action OnJumpPressed;
    public event System.Action<float> OnCycleWeapon;

    private PlayerInput playerInput;
    private InputAction moveXAction;
    private InputAction moveYAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction runAction;
    private InputAction attackAction;
    private InputAction cycleAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        
        var actions = playerInput.actions;
        if (actions == null) return;
        
        moveXAction = actions["MoveX"];
        moveYAction = actions["MoveY"];
        lookAction = actions["Look"];
        jumpAction = actions["Jump"];
        runAction = actions["Run"];
        attackAction = actions["Attack"];
        cycleAction = actions["CycleWeapon"];
    }

    private void OnEnable()
    {
        if (moveXAction != null) moveXAction.performed += OnMoveX;
        if (moveYAction != null) moveYAction.performed += OnMoveY;
        if (lookAction != null) lookAction.performed += OnLook;
        if (lookAction != null) lookAction.canceled += OnLook;
        if (jumpAction != null) jumpAction.performed += OnJump;
        if (attackAction != null) attackAction.performed += OnAttack;
        if (cycleAction != null) cycleAction.performed += OnCycleWeaponAction;
    }

    private void OnDisable()
    {
        if (moveXAction != null) moveXAction.performed -= OnMoveX;
        if (moveYAction != null) moveYAction.performed -= OnMoveY;
        if (lookAction != null) lookAction.performed -= OnLook;
        if (lookAction != null) lookAction.canceled -= OnLook;
        if (jumpAction != null) jumpAction.performed -= OnJump;
        if (attackAction != null) attackAction.performed -= OnAttack;
        if (cycleAction != null) cycleAction.performed -= OnCycleWeaponAction;
    }

    private void LateUpdate()
    {
        RunHeld = runAction?.IsPressed() == true;
    }

    private void OnMoveX(InputAction.CallbackContext ctx)
    {
        moveInput.x = ctx.ReadValue<float>();
        OnMoveInput?.Invoke(moveInput);
    }

    private void OnMoveY(InputAction.CallbackContext ctx)
    {
        moveInput.y = ctx.ReadValue<float>();
        OnMoveInput?.Invoke(moveInput);
    }

    private void OnLook(InputAction.CallbackContext ctx)
    {
        lookInput = ctx.ReadValue<Vector2>();
        OnLookInput?.Invoke(lookInput);
    }

    private void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) OnJumpPressed?.Invoke();
    }

    private void OnAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            GetComponent<WeaponManager>()?.HandleAttackInput();
        }
    }

    private void OnCycleWeaponAction(InputAction.CallbackContext ctx)
    {
        float scroll = ctx.ReadValue<float>();
        OnCycleWeapon?.Invoke(scroll);
    }
}