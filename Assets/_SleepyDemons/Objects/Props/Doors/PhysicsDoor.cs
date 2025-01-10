using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(HingeJoint))]
public class PhysicsDoor : MonoBehaviour, IHoldable
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
    [SerializeField] private float boxColliderScalar = 1.0f;
    
    private Rigidbody _rb;
    private HingeJoint _hingeJoint;
    private BoxCollider _boxCollider;
    private Vector3 _axis;

    void Awake() {
        _rb = GetComponent<Rigidbody>();
        _hingeJoint = GetComponent<HingeJoint>();
        _boxCollider = GetComponent<BoxCollider>();
        _isHolding = false;
        
        CalculatePivot();
        CalculateBoxCollider();
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
        // _rb.angularDrag = 5f;
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
        return (_rb.velocity.magnitude - 0.0f) / (_maxVelocity - 0);
    }

    private void CalculatePivot()
    {
        var center = _boxCollider.center;
        _hingeJoint.anchor = new Vector3(0.5f, center.y, center.z);
        _hingeJoint.axis = new Vector3(0, 1, 0);
    }

    private void CalculateBoxCollider()
    {
        // var size = _boxCollider.size;
        // _boxCollider.size = new Vector3(size.x * boxColliderScalar, size.y * boxColliderScalar, size.z);
        _boxCollider.size *= boxColliderScalar;
    }
}
