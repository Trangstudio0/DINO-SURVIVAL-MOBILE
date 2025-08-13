using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 2f;

    Vector3 velocity;
    bool isGrounded;

    public Transform groundCheck;       // Empty object đặt dưới chân
    public float groundDistance = 0.4f; // Bán kính kiểm tra chạm đất
    public LayerMask groundMask;        // Layer của mặt đất

    void Update()
    {
        // Kiểm tra chạm đất
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f; // Giữ dính đất

        // Lấy input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        // Di chuyển (có tính va chạm)
        controller.Move(move * speed * Time.deltaTime);

        // Nhảy
        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        // Gravity
        velocity.y += gravity * Time.deltaTime;

        // Áp dụng gravity (có tính va chạm)
        controller.Move(velocity * Time.deltaTime);

        // Nếu rơi khỏi map → reset vị trí
        if (transform.position.y < -50f)
            transform.position = new Vector3(0, 10, 0);
    }
}
