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

    private InputManager inputManager;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputManager = GetComponent<InputManager>();

        if (cameraTransform == null)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
                cameraTransform = mainCamera.transform;
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleLook();
        HandleJump();
    }

    private void HandleMovement()
    {
        Vector2 moveInput = inputManager.MoveInput;
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        move = transform.TransformDirection(move);

        float speed = moveSpeed;
        if (inputManager.RunHeld)
        {
            speed *= runMultiplier;
        }

        move *= speed;

        velocity.x = move.x;
        velocity.z = move.z;
        velocity.y -= gravityScale * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
        isGrounded = controller.isGrounded;
    }

    private void HandleLook()
    {
        if (!cameraTransform || (GameManager.Instance.cursorLocked && !GameManager.Instance.isGamePaused))
        {
            Vector2 lookInput = inputManager.LookInput;

            transform.Rotate(Vector3.up, lookInput.x * mouseSensitivity);

            currentLookAngleX -= lookInput.y * mouseSensitivity;
            currentLookAngleX = Mathf.Clamp(currentLookAngleX, minLookAngle, maxLookAngle);

            cameraTransform.localRotation = Quaternion.Euler(currentLookAngleX, 0, 0);
        }
    }

    private void HandleJump()
    {
        if (isGrounded && inputManager.JumpPressed)
        {
            velocity.y = jumpForce;
        }
    }

    public bool IsGrounded() => isGrounded;
    public Vector3 GetVelocity() => velocity;
}