using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [SerializeField] private float _strength;
    [SerializeField] private float _xMax;
    [SerializeField] private float _xMin;
    private StarterAssetsInputs _starterAssetsInput;
    private Animator _animator;
    private float lerpedAnimation;
    private float ylerpedAnimation;

    void Start() {
        _starterAssetsInput = GetComponentInParent<StarterAssetsInputs>();
        _animator = GetComponent<Animator>();
        lerpedAnimation = 0.0f;
    }

    void Update() {
        lerpedAnimation = Mathf.Lerp(lerpedAnimation, _starterAssetsInput.move.x, Time.deltaTime * _strength);
        ylerpedAnimation = Mathf.Lerp(ylerpedAnimation, _starterAssetsInput.move.y, Time.deltaTime * _strength);
        _animator.SetFloat("walking-x", Mathf.Clamp(lerpedAnimation, -1, 1));
        _animator.SetFloat("walking-y", Mathf.Clamp(ylerpedAnimation, -1, 1));
    }
}
