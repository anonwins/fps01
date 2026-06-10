using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private PlayerInput playerInput;

    public Vector2 MoveInput { get; private set; } = Vector2.zero;
    public Vector2 LookInput { get; private set; } = Vector2.zero;
    public bool JumpPressed { get; private set; } = false;
    public bool RunHeld { get; private set; } = false;
    public bool AttackPressed { get; private set; } = false;
    public bool MeleePressed { get; private set; } = false;
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

        playerInput = GetComponent<PlayerInput>();
    }

    public void OnMove(InputValue value)
    {
        MoveInput = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        LookInput = value.Get<Vector2>();
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

    public void OnMelee(InputAction.CallbackContext context)
    {
        if (context.started) MeleePressed = true;
    }

    public void OnCycleWeapon(InputValue value)
    {
        CycleWeaponInput = value.Get<float>();
    }

    private void LateUpdate()
    {
        // Reset single-frame inputs
        JumpPressed = false;
        AttackPressed = false;
        MeleePressed = false;
    }
}
