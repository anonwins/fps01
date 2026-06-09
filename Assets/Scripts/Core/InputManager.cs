using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction runAction;
    private InputAction attackAction;
    private InputAction meleeAction;
    private InputAction cycleWeaponAction;

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
        SetupInputActions();
    }

    private void SetupInputActions()
    {
        var actionMap = playerInput.actions["Player"];
        moveAction = actionMap["Move"];
        lookAction = actionMap["Look"];
        jumpAction = actionMap["Jump"];
        runAction = actionMap["Run"];
        attackAction = actionMap["Attack"];
        meleeAction = actionMap["Melee"];
        cycleWeaponAction = actionMap["CycleWeapon"];
    }

    private void Update()
    {
        MoveInput = moveAction.ReadValue<Vector2>();
        LookInput = lookAction.ReadValue<Vector2>();
        JumpPressed = jumpAction.triggered;
        RunHeld = runAction.IsPressed();
        AttackPressed = attackAction.triggered;
        MeleePressed = meleeAction.triggered;
        CycleWeaponInput = cycleWeaponAction.ReadValue<float>();
    }
}