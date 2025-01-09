using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class FpsArmsAnimatorController : MonoBehaviour
{
    [SerializeField] private float _strength;
    private Animator _animator;
    private float _transition;
    private float _lerpedTransition;
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            _transition = PlayerManager.instance.isAiming ? 1f : 0f;
            _lerpedTransition = Mathf.Lerp(_lerpedTransition, _transition, Time.deltaTime * _strength);
            _animator.SetFloat("AimBlendTree", _lerpedTransition);
        }
    }
}
