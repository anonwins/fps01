using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour, PlayerInputActions.IPlayerActions
{
    public static InputManager Instance { get; private set; }

    private PlayerInputActions inputActions;

    public Vector2 MoveInput { get; private set; } = Vector2.zero;
    public Vector2 LookInput { get; private set; } = Vector2.zero;
    public bool JumpPressed { get; private set; } = false;
    public bool RunHeld { get; private set; } = false;
    public bool AttackPressed { get; private set; } = false;
    public float CycleWeaponInput { get; private set; } = 0f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        inputActions = new PlayerInputActions();
        inputActions.Player.AddCallbacks(this);
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    // IPlayerActions interface implementation
    public void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        LookInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started) JumpPressed = true;
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        RunHeld = context.ReadValueAsButton();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started) AttackPressed = true;
    }

    public void OnCycleWeapon(InputAction.CallbackContext context)
    {
        CycleWeaponInput = context.ReadValue<float>();
    }

    private void LateUpdate()
    {
        // Reset single-frame inputs
        JumpPressed = false;
        AttackPressed = false;
    }
}
