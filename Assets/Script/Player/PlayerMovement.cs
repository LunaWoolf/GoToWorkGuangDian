using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float gravity = -9.81f;
    public float stairHeight = 0.5f; // Maximum height the player can step up on

    private CharacterController controller;
    private SpriteRenderer spriteRenderer;
    private Vector3 velocity;
    private bool isGrounded;

    public float isFliped = 1;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
    }

    void Update()
    {
        // Check if the player is on the ground
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0f;
        }

        // Get movement input from arrow keys
        float moveX = Input.GetAxis("Horizontal"); // Left/Right arrow keys
        float moveZ = Input.GetAxis("Vertical");   // Up/Down arrow keys

        // Flip sprite based on horizontal movement
        if (moveX < 0)
        {
            spriteRenderer.flipX = false;  // Face left
            isFliped = 1;
        }
        else if (moveX > 0)
        {
            spriteRenderer.flipX = true; // Face right
            isFliped = -1;
        }

        // Calculate movement direction
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Apply movement
        controller.Move(move * moveSpeed * Time.deltaTime);

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Handle stepping up on stairs
        StepUpOnStairs();
    }

    void StepUpOnStairs()
    {
        RaycastHit hit;

        // Cast a ray slightly in front of the player to detect steps/stairs
        Vector3 rayStart = transform.position + new Vector3(0, 0.1f, 0); // Start slightly above ground
        Vector3 rayDirection = transform.forward * 0.5f; // Ray direction forward

        if (Physics.Raycast(rayStart, rayDirection, out hit, 1f))
        {
            // If the player is about to walk into something within stair height, move them up
            if (hit.normal.y >= 0.7f && hit.point.y - transform.position.y <= stairHeight)
            {
                controller.Move(Vector3.up * (hit.point.y - transform.position.y)); // Step up to stair
            }
        }
    }
}
