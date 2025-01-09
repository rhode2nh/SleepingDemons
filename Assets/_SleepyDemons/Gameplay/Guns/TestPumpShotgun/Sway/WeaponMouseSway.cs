using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class WeaponMouseSway : MonoBehaviour
{
    [SerializeField] private float _strength;
    [SerializeField] float _xMin;
    [SerializeField] float _xMax;
    
    private Animator _animator;
    private StarterAssetsInputs _starterAssetsInputs;
    private Vector2 _look;
    private Vector2 _lerpedLookDir;

    // Start is called before the first frame update
    void Start()
    {
        _starterAssetsInputs = GetComponentInParent<StarterAssetsInputs>();       
        _animator = GetComponent<Animator>();
        _lerpedLookDir = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        _look = _starterAssetsInputs.look;
        _look = new Vector2(Mathf.Clamp(_look.x, _xMin, _xMax), Mathf.Clamp(_look.y, _xMin, _xMax));
        _lerpedLookDir = Vector2.Lerp(_lerpedLookDir, _look, Time.deltaTime * _strength);
        _animator.SetFloat("x", _lerpedLookDir.x);
        _animator.SetFloat("y", _lerpedLookDir.y);
    }
}
