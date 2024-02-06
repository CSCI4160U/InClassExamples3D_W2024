using UnityEngine;

[RequireComponent (typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private float movementSpeed = 15f;

    [Header("Falling")]
    [SerializeField] private float gravityFactor = 1f;
    [SerializeField] private Transform groundPosition;
    [SerializeField] private LayerMask groundLayers;

    [Header("Jumping")]
    [SerializeField] private bool canAirControl = true;
    [SerializeField] private float jumpSpeed = 7f;

    [Header("Looking")]
    [SerializeField] private float mouseSensitivity = 1000f;
    [SerializeField] private Transform camera;

    private CharacterController characterController;

    private float verticalRotation = 0f;
    public float verticalSpeed = 0f;
    public bool isGrounded = false;

    private void Start() {
        characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        verticalRotation = 0f;
    }

    private void Update() {
        // ground check
        RaycastHit collision;
        if (Physics.Raycast(groundPosition.position, Vector3.down, out collision, 0.1f, groundLayers)) {
            isGrounded = true;
        } else {
            isGrounded = false;
        }

        // update vertical speed
        if (isGrounded) {
            verticalSpeed = 0f;
        } else {
            verticalSpeed += gravityFactor * -9.81f * Time.deltaTime;
        }

        // character rotation (Y axis)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);

        // camera rotation (X axis)
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        camera.localEulerAngles = new Vector3(verticalRotation, 0f, 0f);

        // movement
        Vector3 x = Vector3.zero;
        Vector3 y = Vector3.zero;
        Vector3 z = Vector3.zero;

        // jumping
        if (!isGrounded) {
            y = transform.up * verticalSpeed;
        } else if (Input.GetButtonDown("Jump")) { 
            verticalSpeed = jumpSpeed;
            isGrounded = false;
            y = transform.up * verticalSpeed;
        }

        // walking/running
        if (isGrounded || canAirControl) {
            x = transform.right * Input.GetAxis("Horizontal") * movementSpeed;
            z = transform.forward * Input.GetAxis("Vertical") * movementSpeed;
        }

        characterController.Move((x + y + z) * Time.deltaTime);
    }
}
