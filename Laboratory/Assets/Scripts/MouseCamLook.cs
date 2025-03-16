using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseCamLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    float xRotation = 0f;

    [SerializeField] private InputActionReference cameraRotateInput;

    Vector2 rotateVelocity;

    void Start()
    {
        cameraRotateInput.action.performed += Rotate;
        cameraRotateInput.action.canceled += Rotate;

        Cursor.lockState = CursorLockMode.Locked;

#if UNITY_EDITOR
        var gameWindow = UnityEditor.EditorWindow.GetWindow(typeof(UnityEditor.EditorWindow).Assembly.GetType("UnityEditor.GameView"));
        gameWindow.Focus();
        gameWindow.SendEvent(new Event
        {
            button = 0,
            clickCount = 1,
            type = EventType.MouseDown,
            mousePosition = gameWindow.rootVisualElement.contentRect.center
        });
#endif
    }

    void Update()
    {
        xRotation -= rotateVelocity.y * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * rotateVelocity.x * Time.deltaTime);
    }

    // 입력값을 토대로 캐릭터의 xz속도 조절
    void Rotate(InputAction.CallbackContext context)
    {
        Debug.Log(context.ReadValue<Vector2>());
        rotateVelocity = context.ReadValue<Vector2>() * mouseSensitivity;
    }
}