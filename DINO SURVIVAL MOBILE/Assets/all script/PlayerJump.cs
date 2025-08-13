using UnityEngine;

public class PlayerJumpArk : MonoBehaviour
{
    public CharacterController controller;
    public Transform groundCheck;
    public LayerMask groundMask;

    [Header("Move & Rotate")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 12f;

    [Header("Jump & Gravity")]
    public float jumpHeight = 1.4f;
    public float gravity = -30f;
    public float fallMultiplier = 2.8f;

    [Header("Jump Feel")]
    public float coyoteTime = 0.12f;      // đệm nhảy sau khi rời đất
    public float jumpBufferTime = 0.12f;  // đệm bấm nhảy trước khi chạm đất

    Vector3 velocity;     // vận tốc Y
    Vector2 moveInput;    // lấy từ joystick
    float coyoteCounter;
    float jumpBufferCounter;
    Camera mainCam;

    void Start()
    {
        if (controller == null)
            controller = GetComponent<CharacterController>();

        if (groundCheck == null)
            Debug.LogError("Bạn chưa set groundCheck transform");

        mainCam = Camera.main;

        if (gravity > 0)
            gravity = -gravity;
    }

    void Update()
    {
        float dt = Time.deltaTime;

        bool isGrounded = Physics.CheckSphere(groundCheck.position, 0.25f, groundMask);

        if (isGrounded)
        {
            coyoteCounter = coyoteTime;
            if (velocity.y < 0f) velocity.y = -2f;
        }
        else
        {
            coyoteCounter -= dt;
        }

        if (jumpBufferCounter > 0f) jumpBufferCounter -= dt;

        // Tính hướng di chuyển dựa vào camera
        Vector3 moveDir = Vector3.zero;
        if (mainCam != null)
        {
            Vector3 camForward = Vector3.Scale(mainCam.transform.forward, new Vector3(1,0,1)).normalized;
            Vector3 camRight = mainCam.transform.right;
            moveDir = camForward * moveInput.y + camRight * moveInput.x;
            if (moveDir.magnitude > 1f) moveDir.Normalize();
        }
        else
        {
            moveDir = new Vector3(moveInput.x, 0f, moveInput.y);
        }

        float controlFactor = isGrounded ? 1f : 0.7f;
        Vector3 lateralVelocity = moveDir * moveSpeed * controlFactor;
        controller.Move(lateralVelocity * dt);

        if (moveDir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(new Vector3(moveDir.x, 0, moveDir.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * dt);
        }

        // Trọng lực và rơi nhanh
        if (velocity.y < 0f)
            velocity.y += gravity * fallMultiplier * dt;
        else
            velocity.y += gravity * dt;

        if (jumpBufferCounter > 0f && coyoteCounter > 0f)
        {
            Jump();
            jumpBufferCounter = 0f;
            coyoteCounter = 0f;
        }

        controller.Move(velocity * dt);
    }

    public void SetMoveInput(Vector2 input)
    {
        moveInput = input;
    }

    public void OnJumpPressed()
    {
        jumpBufferCounter = jumpBufferTime;
    }

    void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }
}
