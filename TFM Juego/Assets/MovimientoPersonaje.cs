using UnityEngine;

public class ThirdPersonUserControl : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float turnSpeed = 360f;
    public float jumpHeight = 2f;

    [Header("Gravity Settings")]
    public float gravity = 9.8f;

    [Header("References")]
    public Transform cameraTransform; // Asigna la cámara en el Inspector

    private Vector3 velocity;
    private bool isGrounded;

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        // Check if the player is grounded
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1.1f))
        {
            isGrounded = true;
            if (velocity.y < 0)
            {
                velocity.y = 0f; // Reset vertical velocity when grounded
            }
        }
        else
        {
            isGrounded = false;
        }

        // Get input for movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Get camera forward and right directions
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        // Flatten the directions to ignore vertical tilt
        cameraForward.y = 0f;
        cameraRight.y = 0f;

        // Normalize the directions
        cameraForward.Normalize();
        cameraRight.Normalize();

        // Calculate the movement direction relative to the camera
        Vector3 moveDirection = cameraForward * vertical + cameraRight * horizontal;

        // Rotate the character to face the movement direction
        if (moveDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }

        // Apply movement
        Vector3 move = moveDirection * moveSpeed * Time.deltaTime;
        transform.Translate(move, Space.World);

        // Handle jump
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * 2f * gravity);
        }

        // Apply gravity
        velocity.y -= gravity * Time.deltaTime;

        // Apply vertical movement
        transform.Translate(Vector3.up * velocity.y * Time.deltaTime, Space.World);
    }
}
