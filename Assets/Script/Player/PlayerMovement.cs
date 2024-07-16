using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInput inputPlayer;
    private InputAction actionMove;
    public float speed;
    public Transform cameraTransform;
    // Start is called before the first frame update
    void Start()
    {
        speed = 5;
        inputPlayer= GetComponent<PlayerInput>();
        actionMove = inputPlayer.actions.FindAction("Move");
        cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        // Đọc giá trị đầu vào từ hành động
        Vector2 direction = actionMove.ReadValue<Vector2>();

        // Chuyển đổi hướng di chuyển từ 2D sang 3D
        Vector3 move = new Vector3(direction.x, 0, direction.y);

        // Điều chỉnh hướng di chuyển dựa trên hướng của camera
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // Làm phẳng các thành phần y để tránh di chuyển theo trục y
        forward.y = 0;
        right.y = 0;

        // Chuẩn hóa vector
        forward.Normalize();
        right.Normalize();

        // Tính toán hướng di chuyển cuối cùng
        Vector3 adjustedMove = forward * move.z + right * move.x;

        // Áp dụng di chuyển tới vị trí của nhân vật
        transform.position += adjustedMove * speed * Time.deltaTime;
    }
}
