using System;
using Cinemachine;
using UnityEngine;

public class CameraSettings : MonoBehaviour
{
    private CinemachineVirtualCamera _virtualCamera;

    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void Start()
    {
        if (_virtualCamera != null)
        {
            _virtualCamera.m_Lens.FieldOfView = SaveLoadManager.Instance.CurrentSettings.FieldOfView;
        }
    }
}
