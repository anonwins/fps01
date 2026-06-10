using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public Vector2 MoveInput { get; private set; } = Vector2.zero;
    public Vector2 LookInput { get; private set; } = Vector2.zero;
    public bool RunHeld { get; private set; } = false;

    public event System.Action<Vector2> OnMoveInput;
    public event System.Action<Vector2> OnLookInput;
    public event System.Action OnJumpPressed;
    public event System.Action<float> OnCycleWeapon;

    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction runAction;
    private InputAction attackAction;
    private InputAction cycleAction;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        playerInput = GetComponent<PlayerInput>();
        
        var actions = playerInput.actions;
        if (actions != null)
        {
            moveAction = actions["Move"];
            lookAction = actions["Look"];
            jumpAction = actions["Jump"];
            runAction = actions["Run"];
            attackAction = actions["Attack"];
            cycleAction = actions["CycleWeapon"];
        }
    }

    private void OnEnable()
    {
        if (moveAction != null) moveAction.performed += OnMove;
        if (lookAction != null) lookAction.performed += OnLook;
        if (lookAction != null) lookAction.canceled += OnLook;
        if (jumpAction != null) jumpAction.performed += OnJump;
        if (runAction != null) runAction.performed += OnRun;
        if (runAction != null) runAction.canceled += OnRun;
        if (attackAction != null) attackAction.performed += OnAttack;
        if (cycleAction != null) cycleAction.performed += OnCycleWeaponAction;
    }

    private void OnDisable()
    {
        if (moveAction != null) moveAction.performed -= OnMove;
        if (lookAction != null) lookAction.performed -= OnLook;
        if (lookAction != null) lookAction.canceled -= OnLook;
        if (jumpAction != null) jumpAction.performed -= OnJump;
        if (runAction != null) runAction.performed -= OnRun;
        if (runAction != null) runAction.canceled -= OnRun;
        if (attackAction != null) attackAction.performed -= OnAttack;
        if (cycleAction != null) cycleAction.performed -= OnCycleWeaponAction;
    }

    private void LateUpdate()
    {
        if (runAction != null)
            RunHeld = runAction.IsPressed();
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        MoveInput = ctx.ReadValue<Vector2>();
        OnMoveInput?.Invoke(MoveInput);
    }

    private void OnLook(InputAction.CallbackContext ctx)
    {
        LookInput = ctx.ReadValue<Vector2>();
        OnLookInput?.Invoke(LookInput);
    }

    private void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) OnJumpPressed?.Invoke();
    }

    private void OnRun(InputAction.CallbackContext ctx)
    {
    }

    private void OnAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) WeaponManager.Instance?.HandleAttackInput();
    }

    private void OnCycleWeaponAction(InputAction.CallbackContext ctx)
    {
        float scroll = ctx.ReadValue<float>();
        OnCycleWeapon?.Invoke(scroll);
    }

    public Vector2 ConsumeMoveInput()
    {
        var move = MoveInput;
        MoveInput = Vector2.zero;
        return move;
    }
}