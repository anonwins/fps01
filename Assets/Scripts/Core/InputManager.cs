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
    }

    private void Update()
    {
        if (playerInput == null)
        {
            playerInput = GetComponent<PlayerInput>();
            if (playerInput == null) return;
        }

        // Poll values from PlayerInput
        if (playerInput.actions["Move"] != null)
            MoveInput = playerInput.actions["Move"].ReadValue<Vector2>();
        
        if (playerInput.actions["Look"] != null)
            LookInput = playerInput.actions["Look"].ReadValue<Vector2>();
        
        if (playerInput.actions["Jump"] != null)
            JumpPressed = playerInput.actions["Jump"].triggered;
        
        if (playerInput.actions["Run"] != null)
            RunHeld = playerInput.actions["Run"].IsPressed();
        
        if (playerInput.actions["Attack"] != null)
            AttackPressed = playerInput.actions["Attack"].triggered;
        
        if (playerInput.actions["CycleWeapon"] != null)
            CycleWeaponInput = playerInput.actions["CycleWeapon"].ReadValue<float>();
    }
}
