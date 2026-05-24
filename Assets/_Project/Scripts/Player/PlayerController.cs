using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public Transform playerCamera;
    public float moveSpeed = GameBalance.PlayerMoveSpeed;
    public float mouseSensitivity = GameSettings.DefaultMouseSensitivity;
    public float gravity = GameBalance.PlayerGravity;
    public float moveAcceleration = GameBalance.PlayerMoveAcceleration;
    public float moveDeceleration = GameBalance.PlayerMoveDeceleration;
    public float groundStickVelocity = GameBalance.PlayerGroundStickVelocity;
    public float pitchLimit = GameBalance.PlayerPitchLimit;

    private CharacterController characterController;
    private float cameraPitch;
    private float verticalVelocity;
    private Vector3 horizontalVelocity;

    public Vector3 CurrentHorizontalVelocity => horizontalVelocity;

    private void Awake()
    {
        GameSettings.Load();
        characterController = GetComponent<CharacterController>();
        if (playerCamera == null && Camera.main != null)
        {
            playerCamera = Camera.main.transform;
        }
    }

    private void Update()
    {
        if (GameStateController.Instance != null && !GameStateController.Instance.IsGameplayActive)
        {
            return;
        }

        Look();
        Move();
    }

    private void Look()
    {
        mouseSensitivity = GameSettings.MouseSensitivity;
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        cameraPitch = Mathf.Clamp(cameraPitch - mouseY, -pitchLimit, pitchLimit);
        if (playerCamera != null)
        {
            playerCamera.localEulerAngles = Vector3.right * cameraPitch;
        }
    }

    private void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 input = new Vector3(horizontal, 0f, vertical);
        input = Vector3.ClampMagnitude(input, 1f);

        Vector3 targetHorizontalVelocity = transform.TransformDirection(input) * moveSpeed;
        float accelerationRate = input.sqrMagnitude > 0.001f ? moveAcceleration : moveDeceleration;
        horizontalVelocity = Vector3.MoveTowards(horizontalVelocity, targetHorizontalVelocity, accelerationRate * Time.deltaTime);

        if (characterController.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = groundStickVelocity;
        }

        verticalVelocity += gravity * Time.deltaTime;
        Vector3 worldMove = horizontalVelocity;
        worldMove.y = verticalVelocity;

        characterController.Move(worldMove * Time.deltaTime);
    }
}
