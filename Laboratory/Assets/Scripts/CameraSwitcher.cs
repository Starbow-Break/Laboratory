using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private List<CinemachineCamera> cinemachineCameras;

    public static CameraSwitcher instance { get; private set; }
    public static CinemachineCamera activeCamera { get; private set; }

    private readonly int ActivePriority = 10;
    private readonly int InActivePriority = 0;

    void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        foreach (CinemachineCamera cinemachineCamera in cinemachineCameras)
        {
            if (cinemachineCamera.Priority == ActivePriority)
            {
                activeCamera = cinemachineCamera;
                break;
            }
        }
    }

    private void OnDisable()
    {
        if (instance == this)
        {
            instance = null;
        }

        activeCamera = null;
    }

    public void SwitchCamera(string cameraName)
    {
        foreach (CinemachineCamera camera in cinemachineCameras)
        {
            if (camera.name == cameraName)
            {
                camera.Priority = ActivePriority;
                activeCamera = camera;
            }
            else
            {
                camera.Priority = InActivePriority;
            }
        }
    }
}
