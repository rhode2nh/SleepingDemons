using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using StarterAssets;
using UnityEngine;

public class WeaponAim : MonoBehaviour
{
    [SerializeField] private float _zoomFOV;
    [SerializeField] private float _zoomStrength;

    private float _initialFOV;
    private float _lerpedFOV;
    private float _lerpToFOV;
    private CinemachineVirtualCamera _virtualCamera;
    private Animator _animator;
    private bool _isAiming;
    
    // Start is called before the first frame update
    void Start()
    {
        _virtualCamera = GetComponentInParent<CinemachineVirtualCamera>();
        _initialFOV = SaveLoadManager.Instance.CurrentSettings.FieldOfView;
        _lerpedFOV = _initialFOV;
        _lerpToFOV = _zoomFOV;
        _animator = GetComponent<Animator>();
        _isAiming = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isAiming != PlayerManager.Instance.IsAiming)
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
        
        _isAiming = PlayerManager.Instance.IsAiming;
        Zoom();
    }

    void Zoom()
    {
        _lerpToFOV = PlayerManager.Instance.IsAiming ? _zoomFOV : _initialFOV;
        _lerpedFOV = Mathf.Lerp(_lerpedFOV, _lerpToFOV, Time.deltaTime * _zoomStrength);
        _virtualCamera.m_Lens.FieldOfView = _lerpedFOV;
    }
}
