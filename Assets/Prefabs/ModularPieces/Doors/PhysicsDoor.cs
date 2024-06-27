using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class PhysicsDoor : MonoBehaviour, IHoldable, IInteractable
{
    [SerializeField] private bool _isHolding;
    [SerializeField] private bool _flipAxis;
    [SerializeField] private bool _isMoving;
    [SerializeField] private bool _isCreeking;
    [SerializeField] private float _strength;
    [SerializeField] private AudioSource creek;
    [SerializeField] private float _minPitch;
    [SerializeField] private float _maxPitch;
    [SerializeField] private float _maxVelocity;
    
    private Rigidbody _rb;
    private Vector3 axis;

    void Start() {
        _rb = GetComponentInParent<Rigidbody>();
        _isHolding = false;
    }

    public void Hold(Vector3 hitPoint, bool isHolding) {
        _isHolding = isHolding;
        if (_isHolding) {
            StartCoroutine(HoldObject(hitPoint));
        }
    }

    public void Nudge(float strength)
    {
        if (_flipAxis)
        {
            if (_flipAxis) {
                _rb.AddTorque(-transform.up * strength, ForceMode.Impulse);
            } else {
                _rb.AddTorque(transform.up * strength, ForceMode.Impulse);
            }
        }
    }

    IEnumerator HoldObject(Vector3 hitPoint) {
        var relativeDistance = Vector3.Distance(Camera.main.transform.position, hitPoint);
        var intiialReferencePoint = (Camera.main.transform.position + Camera.main.transform.forward * relativeDistance - transform.position).normalized;
        var referenceDot = Vector3.Dot(transform.forward, intiialReferencePoint);
        _rb.angularDrag = 5f;
        while (PlayerManager.instance.isHolding) {
            var referencePoint = (Camera.main.transform.position + Camera.main.transform.forward * relativeDistance - transform.position).normalized;
            var dotProduct = Vector3.Dot(transform.forward, referencePoint) - referenceDot;
            if (_flipAxis) {
                _rb.AddTorque(-transform.up * _strength * dotProduct);
            } else {
                _rb.AddTorque(transform.up * _strength * dotProduct);
            }
            _rb.AddTorque(-_rb.angularVelocity);

            yield return new WaitForFixedUpdate();
        }
        _rb.angularDrag = 0.5f;
    }

    void PlayCreek() {
        if (_rb.velocity.magnitude >= 0.01f) {
            _isMoving = true;
        } else {
            _isMoving = false;
            _isCreeking = false;
            creek.Stop();
        }

        if (_isMoving && !_isCreeking) {
            creek.Play();           
            _isCreeking = true;
        }

        if (_isMoving) {
            creek.volume = _rb.velocity.magnitude;
            creek.pitch = Mathf.Clamp(_rb.velocity.magnitude, _minPitch, _maxPitch);
        }
    }

    public float GetVelocity() {
        return _rb.velocity.magnitude;
    }

    public bool IsHolding() {
        return _isHolding;
    }

    public float GetNormalizedVelocity() {
        // return Mathf.Lerp(0.0f, _maxVelocity, _rb.velocity.magnitude);
        return (_rb.velocity.magnitude - 0.0f) / (_maxVelocity - 0);
    }

    public void ExecuteInteraction(GameObject other)
    {
        Hold(other.GetComponent<Interact>()._inputRaycast.hit.point, PlayerManager.instance.isHolding);
    }

    public bool ExecuteOnRelease() { return true; }
}
