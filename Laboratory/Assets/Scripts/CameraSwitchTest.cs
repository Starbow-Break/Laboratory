using UnityEngine;

public class CameraSwitchTest : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CameraSwitcher.instance.SwitchCamera("Interaction Cam");            
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CameraSwitcher.instance.SwitchCamera("Player Sight Cam");            
        }
    }
}
