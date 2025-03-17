using Unity.Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CinemachineCamera playerCamera; // 플레이어의 카메라 트랜스폼
    [SerializeField] private float moveSpeed = 5.0f; // 이동 속도
    [SerializeField] private float rotateSpeed = 60.0f; // 회전 속도
    [SerializeField] private float xRotationBound = 60.0f; // x축 회전 각도 제한
    
    private Rigidbody rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 입력
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");
        
        // 플레이어 이동 및 회전
        rb.position += (transform.forward * vertical + transform.right * horizontal).normalized * moveSpeed * Time.deltaTime;
        transform.Rotate(0, mouseX * rotateSpeed * Time.deltaTime, 0);
        
        // 카메라 회전
        float xRotation = playerCamera.transform.eulerAngles.x - mouseY * rotateSpeed * Time.deltaTime;
        if (xRotationBound < xRotation && xRotation < 180.0f)
        {
            xRotation = xRotationBound;
        }
        else if (xRotation < 360.0f - xRotationBound && 180.0f < xRotation)
        {
            xRotation = 360.0f - xRotationBound;
        }
        playerCamera.transform.localEulerAngles = new Vector3(xRotation, 0, 0);
    }
}
