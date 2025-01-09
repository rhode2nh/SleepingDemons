using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawer : MonoBehaviour, IHoldable
{
    [SerializeField] private float _strength;
    [SerializeField] private bool _isHolding;

    private Rigidbody _rb;

    void Start() {
        _rb = GetComponent<Rigidbody>();
    }

    public void Hold(Vector3 hitPoint, bool isHolding) {
        _isHolding = isHolding;
        if (_isHolding) {
            StartCoroutine(HoldObject(hitPoint));
        }
    }

    IEnumerator HoldObject(Vector3 hitPoint) {
        var relativeDistance = Vector3.Distance(Camera.main.transform.position, hitPoint);
        var intiialReferencePoint = (Camera.main.transform.position + Camera.main.transform.forward * relativeDistance - transform.position).normalized;
        var referenceDot = Vector3.Dot(transform.forward, intiialReferencePoint);
        _rb.angularDrag = 5f;
        while (_isHolding) {
            var referencePoint = (Camera.main.transform.position + Camera.main.transform.forward * relativeDistance - transform.position).normalized;
            var dotProduct = Vector3.Dot(transform.forward, referencePoint) - referenceDot;
            _rb.AddForce(transform.forward * _strength * dotProduct);
            // _rb.AddTorque(-_rb.angularVelocity);

            yield return new WaitForFixedUpdate();
        }
        _rb.angularDrag = 0.5f;
    }
}
