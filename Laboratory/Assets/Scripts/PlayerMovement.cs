using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform playerCameraTransform;
    public float baseSpeed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float sprintSpeed = 5f;
    [SerializeField] private InputActionReference moveInput;

    float speedBoost = 1f;
    Vector2 moveInputValue;
    Vector3 yVelocity;

    void Start()
    {
        moveInput.action.performed += Move;   
        moveInput.action.canceled += Move;
    }

    void Update()
    {
        if (controller.isGrounded && yVelocity.y < 0)
        {
            yVelocity.y = -2f;
        }

        if (Input.GetButton("Fire3"))
            speedBoost = sprintSpeed;
        else
            speedBoost = 1f;

        Vector3 direction = playerCameraTransform.right * moveInputValue.x + playerCameraTransform.forward * moveInputValue.y;
        controller.Move(direction * (baseSpeed + speedBoost) * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            yVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        yVelocity.y += gravity * Time.deltaTime;

        controller.Move(yVelocity * Time.deltaTime);
    }

    // 입력값을 토대로 캐릭터의 xz속도 조절
    void Move(InputAction.CallbackContext context)
    {
        moveInputValue = context.ReadValue<Vector2>();
    }
}
