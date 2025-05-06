using Cinemachine;
using UnityEngine;

public class POVCamera : MonoBehaviour
{
    [field: SerializeField] public float LeftClamp { get; private set; }
    [field: SerializeField] public float RightClamp { get; private set; }
    [field: SerializeField] public float TopClamp { get; private set; }
    [field: SerializeField] public float BottomClamp { get; private set; }
    
    public CinemachineVirtualCamera VirtualCamera { get; private set; }
    public Transform CameraTransform { get; private set; }

    private void Awake()
    {
        VirtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        CameraTransform = transform;
    }
}
