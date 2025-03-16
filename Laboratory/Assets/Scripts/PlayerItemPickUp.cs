using TMPro;
using Unity.Multiplayer.Center.Common.Analytics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerItemPickUp : MonoBehaviour
{
    [SerializeField] private LayerMask pickableLayerMask;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private TextMeshProUGUI pickUpUIText;
    [SerializeField] private AimUI aimUI;
    [SerializeField, Min(1)] private float hitRange = 3.0f;
    [SerializeField] private Transform pickUpParent;
    [SerializeField] private InputActionReference interactionInput, dropInput;

    private RaycastHit hit;
    private GameObject inHandItem;

    private void Start()
    {
        interactionInput.action.performed += Interact;
        dropInput.action.performed += Drop;
    }

    private void Update()
    {
        // UI 및 하이라이트 초기화
        if(hit.collider != null)
        {
            hit.collider.GetComponent<Highlight>()?.SetHighlight(false);
            pickUpUIText.color = new Color(
                pickUpUIText.color.r,
                pickUpUIText.color.g,
                pickUpUIText.color.b,
                0.0f
            );
            aimUI.SetBigger(false);
        }

        // 이미 아이템이 손에 들려 있다면 아이템을 감지하는 로직은 건너 뛴다.
        if(inHandItem != null)
        {
            return;
        }

        // 에임에 들어온 아이템 감지
        Ray ray = playerCamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        Debug.DrawRay(ray.origin, ray.direction * hitRange, Color.red);
        
        if(Physics.Raycast(
            ray,
            out hit, 
            hitRange, 
            pickableLayerMask
        ))
        {
            hit.collider.GetComponent<Highlight>()?.SetHighlight(true);
            pickUpUIText.color = new Color(
                pickUpUIText.color.r,
                pickUpUIText.color.g,
                pickUpUIText.color.b,
                1.0f
            );
            aimUI.SetBigger(true);
        }
    }

    private void Interact(InputAction.CallbackContext context) {
        
        if (inHandItem)
        {
            Use();
        }
        else if(hit.collider != null)
        {
            PickUp();
        }
    }

    private void PickUp()
    {
        Debug.Log(hit.collider.name);
        Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
        if(hit.collider.GetComponent<Item>())
        {
            Debug.Log("Pick Up Item!");
            inHandItem = hit.collider.gameObject;
            inHandItem.transform.SetParent(pickUpParent.transform, false);
            inHandItem.transform.localPosition = Vector3.zero;
            inHandItem.transform.localRotation = Quaternion.identity;

            if(rb != null)
            {
                rb.isKinematic = true;
            }
        }
    }

    private void Use()
    {
        if(inHandItem != null)
        {
            IUsable usable = inHandItem.GetComponent<IUsable>();
            if (usable != null)
            {
                usable.Use(gameObject);
            }
        }
    }

    private void Drop(InputAction.CallbackContext context)
    {
        if (inHandItem != null)
        {
            Rigidbody rb = inHandItem.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.isKinematic = false;
            }

            inHandItem.transform.SetParent(null);
            inHandItem = null;
        }
    }
}
