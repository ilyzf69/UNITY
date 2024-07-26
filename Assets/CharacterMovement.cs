using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public Animator animator;
    public float walkSpeed = 2.0f;
    public float runSpeed = 5.0f;
    public float jumpForce = 7.0f;
    public float mouseSensitivity = 100.0f;
    public Transform cameraTransform;
    public Transform cameraHolder;

    private Rigidbody rb;
    private GameController gameController;
    private bool isGrounded = true;
    private bool isPaused = false;
    private bool isSprinting = false;
    private bool autoSprint = false;
    private float xRotation = 0.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameController = FindObjectOfType<GameController>();

        // Lock the cursor to the center of the screen initially
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Toggle pause when pressing the Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        // Toggle auto sprint when pressing the P key
        if (Input.GetKeyDown(KeyCode.P))
        {
            autoSprint = !autoSprint;
        }

        if (gameController != null && gameController.isGameActive && !isPaused)
        {
            Rotate();
            Move();
            Jump();
        }
    }

    void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Calculate the direction relative to the camera
        Vector3 direction = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized;
        direction = cameraTransform.TransformDirection(direction);
        direction.y = 0.0f;  // Keep the direction horizontal

        // Determine the speed
        float speed = autoSprint ? runSpeed : walkSpeed;
        if (Input.GetKey(KeyCode.LeftShift) || autoSprint)
        {
            speed = runSpeed;
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }

        Vector3 movement = direction * speed * Time.deltaTime;
        rb.MovePosition(transform.position + movement);

        // Update the "Speed" parameter in the Animator
        animator.SetFloat("Speed", movement.magnitude);

        // Update the "IsSprinting" parameter in the Animator
        animator.SetBool("IsSprinting", isSprinting);
    }

    void Jump()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            animator.SetTrigger("Jump");
        }
    }

    void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        // Make sure the camera holder follows the player's rotation
        cameraHolder.position = transform.position;
        cameraHolder.rotation = transform.rotation;
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    void TogglePause()
    {
        isPaused = !isPaused;
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Time.timeScale = isPaused ? 0 : 1;
    }

    void OnGUI()
    {
        // Display the auto sprint status on the screen
        string sprintStatus = autoSprint ? "Auto Sprint: ON" : "Auto Sprint: OFF";
        GUI.Label(new Rect(10, 10, 150, 20), sprintStatus);
    }
}
