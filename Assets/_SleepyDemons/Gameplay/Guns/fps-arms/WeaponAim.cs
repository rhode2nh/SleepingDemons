using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class WeaponAim : MonoBehaviour
{
    [SerializeField] private float _strength;
    [SerializeField] private float _zoomFOV;
    [SerializeField] private float _zoomStrength;

    private float _initialFOV;
    private float _lerpedFOV;
    private float _lerpToFOV;
    private FirstPersonController _firstPersonController;
    private Animator _animator;
    private bool _isAiming;
    
    // Start is called before the first frame update
    void Start()
    {
        _firstPersonController = GetComponentInParent<FirstPersonController>();
        _initialFOV = _firstPersonController.virtualCamera.m_Lens.FieldOfView;
        _lerpedFOV = _initialFOV;
        _lerpToFOV = _zoomFOV;
        _animator = GetComponent<Animator>();
        _isAiming = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isAiming != PlayerManager.instance.isAiming)
        {
            if (_animator.GetCurrentAnimatorStateInfo(4).IsName("Idle"))
            {
                _animator.Play("Aim.Aim", 4, 0);
            }
            else
            {
                float time = _animator.GetCurrentAnimatorStateInfo(4).normalizedTime;
                if (_animator.GetCurrentAnimatorStateInfo(4).IsName("Aim"))
                {
                    _animator.Play("Aim.Stop Aim", 4, Mathf.Clamp(1 - time, 0, _animator.GetCurrentAnimatorStateInfo(4).length));
                }
            }
        }
        
        _isAiming = PlayerManager.instance.isAiming;
        Zoom();
    }

    void Zoom()
    {
        _lerpToFOV = PlayerManager.instance.isAiming ? _zoomFOV : _initialFOV;
        _lerpedFOV = Mathf.Lerp(_lerpedFOV, _lerpToFOV, Time.deltaTime * _zoomStrength);
        _firstPersonController.virtualCamera.m_Lens.FieldOfView = _lerpedFOV;
    }
}
