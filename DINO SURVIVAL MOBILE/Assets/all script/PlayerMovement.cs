using UnityEngine;

public class JoystickPlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;         // tốc độ di chuyển
    public Joystick joystick;           // tham chiếu joystick
    private CharacterController controller;

    private void Start()
    {
        controller = GetComponent<CharacterController>();

        // Nếu chưa gán joystick trong Inspector, tìm tự động
        if (joystick == null)
        {
            joystick = FindObjectOfType<Joystick>();
        }
    }

    private void Update()
    {
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        Vector3 direction = new Vector3(horizontal, 0f, vertical);

        if (direction.magnitude >= 0.1f)
        {
            controller.Move(direction * moveSpeed * Time.deltaTime);
        }
    }
}
