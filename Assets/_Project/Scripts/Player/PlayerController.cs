using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public Transform playerCamera;
    public float moveSpeed = 6.5f;
    public float mouseSensitivity = 2.2f;
    public float gravity = -20f;

    private CharacterController characterController;
    private float cameraPitch;
    private float verticalVelocity;

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

        cameraPitch = Mathf.Clamp(cameraPitch - mouseY, -82f, 82f);
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

        Vector3 worldMove = transform.TransformDirection(input) * moveSpeed;

        if (characterController.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        verticalVelocity += gravity * Time.deltaTime;
        worldMove.y = verticalVelocity;

        characterController.Move(worldMove * Time.deltaTime);
    }
}
