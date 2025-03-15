using UnityEngine;

public class PlayerItemPickUp : MonoBehaviour
{
    [SerializeField] private LayerMask pickableLayerMask;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private GameObject pickUpUI;
    [SerializeField] private AimUI aimUI;
    [SerializeField, Min(1)] private float hitRange = 3.0f;

    private RaycastHit hit;

    private void Update()
    {
        if(hit.collider != null)
        {
            hit.collider.GetComponent<Highlight>()?.SetHighlight(false);
        }

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
            pickUpUI.SetActive(true);
            aimUI.SetBigger(true);
        }
        else
        {
            pickUpUI.SetActive(false);
            aimUI.SetBigger(false);
        }
    }
}
