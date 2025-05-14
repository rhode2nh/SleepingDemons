using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Holdable : Interactable, IHoldable
{
    [SerializeField] protected bool _isHolding;
    [SerializeField] private float _strength;
    [SerializeField] private bool _bringToCenter;
    [SerializeField] private bool modifyVelocityDirectly;
    [SerializeField] private float brakingDistance;
    [SerializeField] private float brakingStrength;
    [SerializeField] private float _holdingDrag = 1.0f;
    [SerializeField] private float _holdingAngularDrag = 1.0f;
    
    private Rigidbody _rb;
    private Vector3 _initialReferencePoint;
    private Vector3 _posToHold;
    private float _initialDrag;
    private float _initialAngularDrag;

     internal virtual void Awake() {
        _rb = GetComponent<Rigidbody>();
        _rb.interpolation = RigidbodyInterpolation.Interpolate;
        _initialReferencePoint = new Vector3();
        _isHolding = false;
        _initialDrag = _rb.linearDamping;
        _initialAngularDrag = _rb.angularDamping;
     }

    public void Hold(Vector3 hitPoint, bool isHolding) {
        _isHolding = isHolding;
        if (_isHolding)
        {
            StartCoroutine(HoldObject(hitPoint));
        }
    }

    IEnumerator HoldObject(Vector3 hitPoint) {
        _rb.linearDamping = _holdingDrag;
        _rb.angularDamping = _holdingAngularDrag;
        var relativeDistance = Vector3.Distance(Camera.main.transform.position, hitPoint);
        _initialReferencePoint = Camera.main.transform.position + Camera.main.transform.forward * relativeDistance;
        var initialPos = _rb.position;
        PlayerManager.Instance.Holdable = gameObject;
        
        while (PlayerManager.Instance.IsHolding)
        {
            _posToHold = Camera.main.transform.position + (Camera.main.transform.forward * (relativeDistance + PlayerManager.Instance.HoldOffset));
            var pointToHold = _bringToCenter ? _rb.position : _initialReferencePoint + (_rb.position - initialPos);
            var force = (_posToHold - pointToHold) * _strength;
            
            Vector3 directionToWaypoint = _posToHold - _rb.position;
            float distanceToWaypoint = directionToWaypoint.magnitude;
        
            
            // What are the reprecussions?
            if (modifyVelocityDirectly)
            {
                _rb.linearVelocity = force;
            }
            else
            {
                if (distanceToWaypoint > brakingDistance)
                {
                    // If we're not close to the waypoint, move towards it
                    _rb.AddForce(force);
                }
                else
                {
                    // Apply braking force when near the waypoint
                    Vector3 brakingForce = -_rb.linearVelocity.normalized * brakingStrength;
                    _rb.AddForce(brakingForce, ForceMode.Force);
                }
            }

            if (PlayerManager.Instance.IsRotating)
            {
                Vector3 rotateY = -Camera.main.transform.up * PlayerManager.Instance.RotateVector.x;
                Vector3 rotateX = -Camera.main.transform.right * PlayerManager.Instance.RotateVector.y;
                _rb.angularVelocity = (rotateY + rotateX) * PlayerManager.Instance.RotateForce;
            }
            
            yield return new WaitForFixedUpdate();
        }
        
        PlayerManager.Instance.Holdable = null;
        _rb.linearDamping = _initialDrag;
        _rb.angularDamping = _initialAngularDrag;
        PlayerManager.Instance.HoldOffset = 0.0f;

        if (PlayerManager.Instance.IsThrowing)
        {
            _rb.AddForce(Camera.main.transform.forward * PlayerManager.Instance.ThrowForce, ForceMode.Impulse);
            PlayerManager.Instance.IsThrowing = false;
        }
    }

    void OnDrawGizmos() {
        if (_isHolding) {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(_posToHold, 0.1f);
        }
    }

    public override void ExecuteInteraction(GameObject other)
    {
        // TODO: Is there some way to get around using the global variable?
        Hold(other.gameObject.GetComponent<Interact>()._inputRaycast.hit.point, PlayerManager.Instance.IsHolding);
    }
}
