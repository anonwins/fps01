using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(InputManager))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float runMultiplier = 1.5f;
    public float jumpForce = 7f;
    public float gravityScale = 3f;
    public float mouseSensitivity = 2f;

    [Header("Look")]
    public Transform cameraTransform;
    public float minLookAngle = -80f;
    public float maxLookAngle = 80f;

    private CharacterController controller;
    private Vector3 velocity;
    private float currentLookAngleX = 0f;
    private bool isGrounded;

    private Vector2 moveInput = Vector2.zero;
    private Vector2 lookInput = Vector2.zero;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        if (cameraTransform == null)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
                cameraTransform = mainCamera.transform;
        }

        var inputMgr = GetComponent<InputManager>();
        inputMgr.OnMoveInput += HandleMoveInput;
        inputMgr.OnLookInput += HandleLookInput;
        inputMgr.OnJumpPressed += HandleJump;
    }

    private void Update()
    {
        HandleMovement();
        HandleLook();
    }

    private void HandleMoveInput(Vector2 input) => moveInput = input;
    private void HandleLookInput(Vector2 input) => lookInput = input;

    private void HandleMovement()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        move = transform.TransformDirection(move);

        var inputMgr = GetComponent<InputManager>();
        float speed = moveSpeed * (inputMgr.RunHeld ? runMultiplier : 1f);
        move *= speed;

        velocity.x = move.x;
        velocity.z = move.z;

        if (controller.isGrounded)
        {
            if (velocity.y < 0) velocity.y = -2f;
        }

        velocity.y -= gravityScale * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        isGrounded = controller.isGrounded;
    }

    private void HandleLook()
    {
        if (!cameraTransform) return;

        bool canLook = GameManager.Instance.cursorLocked && !GameManager.Instance.isGamePaused;
        if (!canLook)
        {
            lookInput = Vector2.zero;
            return;
        }

        Vector2 delta = lookInput;
        lookInput = Vector2.zero;

        if (delta.sqrMagnitude > 0.001f)
        {
            transform.Rotate(Vector3.up, delta.x * mouseSensitivity);
            currentLookAngleX -= delta.y * mouseSensitivity;
            currentLookAngleX = Mathf.Clamp(currentLookAngleX, minLookAngle, maxLookAngle);

            cameraTransform.localRotation = Quaternion.Euler(currentLookAngleX, 0, 0);
        }
    }

    private void HandleJump()
    {
        if (isGrounded)
            velocity.y = jumpForce;
    }

    public bool IsGrounded() => isGrounded;
    public Vector3 GetVelocity() => velocity;
}