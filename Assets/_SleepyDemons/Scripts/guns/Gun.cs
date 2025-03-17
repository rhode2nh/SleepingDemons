using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour, IGun, IEquippable
{
    private Animator _animator;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _force;
    [SerializeField] private float _torque;
    [SerializeField] private float _damage;
    [SerializeField] private int _capacity;
    [SerializeField] private int _currentAmmoCount;
    [SerializeField] private Vector3 _equipPos;
    [SerializeField] private GameObject _animatedMagazine;
    [SerializeField] private GameObject _animatedGun;
    [SerializeField] private GameObject _childMagazine;

    [SerializeField] private Animator _gunAnimator;
    [SerializeField] private AudioSource _gunShot;

    void Start()
    {
        _animator = GetComponent<Animator>();
        // _bulletRaycastPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane));
    }

    public void Aim(bool newState)
    {
        _animator.SetBool("IsAiming", newState);
    }

    public void CheckMagazine(bool newState)
    {
        _animator.SetBool("IsCheckingMagazine", newState);
        _gunAnimator.SetBool("IsCheckingMagazine", newState);
    }
    
    public void CheckChamber(bool newState)
    {
        _animator.SetBool("IsCheckingChamber", newState);
        _gunAnimator.SetBool("IsCheckingChamber", newState);
    }

    public void UnCheckAmmo()
    {
        _animator.SetBool("IsUnChecking", false);
        _gunAnimator.SetBool("IsUnChecking", false);
    }

    public void Attack(bool newState)
    {
        if (_currentAmmoCount == 0) return;

        _currentAmmoCount--;
        _animator.Play("Shoot", -1, 0f); 
        _gunAnimator.Play("Shoot", -1, 0f); 
        _animator.SetBool("IsShooting", newState);
        _gunAnimator.SetBool("IsShooting", newState);
    }

    public void OnShoot()
    {
        Vector3 forwardDir = Camera.main.transform.forward;
        Vector3 rightDir = Camera.main.transform.right;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out var hit, Mathf.Infinity, _layerMask))
        {
            var damageObject = hit.collider.GetComponent<IDamageable>();
            damageObject?.TakeDamage(_damage, forwardDir * _force, rightDir * _torque);
        }

        if (_gunShot.clip != null)
        {
            _gunShot.PlayOneShot(_gunShot.clip);
        }
    }

    public void StopShooting()
    {
        _animator.SetBool("IsShooting", false);
        _gunAnimator.SetBool("IsShooting", false);
    }

    public void LoadBullet()
    {
        if (_currentAmmoCount == _capacity) return;

        _currentAmmoCount++;
        _animator.SetBool("IsLoadingBullet", true);
    }

    public void SwitchToMag(bool newState)
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Check Chamber Idle"))
        {
            _animator.SetBool("IsSwitchingToMag", newState);
            _animator.SetBool("IsCheckingChamber", false);
            InputManager.Instance.SwitchCurrentActionMap("CheckMagazine");
        }
    }

    public void StopSwitchToMag()
    {
        _animator.SetBool("IsSwitchingToMag", false);
    }
    
    public void SwitchToChamber(bool newState)
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Check Magazine"))
        {
            _animator.SetBool("IsSwitchingToChamber", newState);
            _animator.SetBool("IsCheckingMagazine", false);
            InputManager.Instance.SwitchCurrentActionMap("CheckChamber");
        }
    }

    public void StopSwitchToChamber()
    {
        _animator.SetBool("IsSwitchingToChamber", false);
    }

    public void StopLoadBullet()
    {
        _animator.SetBool("IsLoadingBullet", false);
    }

    public void EmptyChamber()
    {
        if (_currentAmmoCount > 0)
        {
            _currentAmmoCount--;
        }
        
        _animator.SetBool("IsEmptyingChamber", true);
        _gunAnimator.SetBool("IsEmptyingChamber", true);
    }

    public void StopEmptyChamber()
    {
        _animator.SetBool("IsEmptyingChamber", false);
        _gunAnimator.SetBool("IsEmptyingChamber", false);
    }

    public Vector3 GetEquipPos()
    {
        return _equipPos;
    }

    public void ShowAnimatedMagazine()
    {
       _animatedMagazine.SetActive(true); 
       _childMagazine.SetActive(false);
    }
    
    public void HideAnimatedMagazine()
    {
       _childMagazine.SetActive(true);
       _animatedMagazine.SetActive(false); 
    }

    public void ShowGun()
    {
        _animatedGun.SetActive(true);       
    }
    
    public void HideGun()
    {
        _animatedGun.SetActive(false);       
    }
}
