using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class FocusOn : MonoBehaviour
{
    public static FocusOn instance;
    public Vector3 focusPoint;
    public bool isFocusing;
    public float focusSpeed;
    public float unfocusSpeed;
    private FirstPersonController firstPersonController;
    private float lerpedFOV;
    private float initialSens;
    private float initialFOV;
    // Start is called before the first frame update
    void Awake() {
        instance = this;
    }
    void Start()
    {
        firstPersonController = GetComponent<FirstPersonController>();
        lerpedFOV = firstPersonController.virtualCamera.m_Lens.FieldOfView;
        initialSens = firstPersonController.RotationSpeed;
        initialFOV = firstPersonController.virtualCamera.m_Lens.FieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        // if (isFocusing) {
        //     lerpedFOV = Mathf.Lerp(lerpedFOV, 60, focusSpeed * Time.deltaTime);
        //     firstPersonController.virtualCamera.m_Lens.FieldOfView = lerpedFOV;

        //     var targetRotation = Quaternion.LookRotation(focusPoint - transform.position);
        //     Quaternion xFocus = Quaternion.Slerp(firstPersonController.transform.rotation, targetRotation, focusSpeed * Time.deltaTime);
        //     firstPersonController.transform.rotation = Quaternion.Euler(0, xFocus.eulerAngles.y, xFocus.eulerAngles.z);
        // } else if (Mathf.Abs(firstPersonController.virtualCamera.m_Lens.FieldOfView - initialFOV) > 0.001f) {
        //     lerpedFOV = Mathf.Lerp(lerpedFOV, initialFOV, unfocusSpeed * Time.deltaTime);
        //     firstPersonController.virtualCamera.m_Lens.FieldOfView = lerpedFOV;
        // }
    }

    public void Focus(Vector3 focusPoint) {
        this.focusPoint = focusPoint;
        isFocusing = true;
        firstPersonController.RotationSpeed = 0.5f;
        firstPersonController.playerInput.SwitchCurrentActionMap("Focused");
    }

    public void UnFocus() {
        this.focusPoint = new Vector3();
        firstPersonController.RotationSpeed = initialSens;
        isFocusing = false;
        firstPersonController.playerInput.SwitchCurrentActionMap("Player");
    }
}
