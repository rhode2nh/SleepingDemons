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
    
    private Rigidbody _rb;
    private Vector3 _initialReferencePoint;
    private Vector3 _posToHold;

     internal virtual void Awake() {
        _rb = GetComponent<Rigidbody>();
        _initialReferencePoint = new Vector3();
        _isHolding = false;
    }

    public void Hold(Vector3 hitPoint, bool isHolding) {
        _isHolding = isHolding;
        if (_isHolding) {
            StartCoroutine(HoldObject(hitPoint));
        }
    }

    IEnumerator HoldObject(Vector3 hitPoint) {
        var relativeDistance = Vector3.Distance(Camera.main.transform.position, hitPoint);
        _initialReferencePoint = Camera.main.transform.position + Camera.main.transform.forward * relativeDistance;
        var initialPos = _rb.position;
        
        while (PlayerManager.Instance.IsHolding)
        {
            _posToHold = Camera.main.transform.position + (Camera.main.transform.forward * relativeDistance);
            var pointToHold = _bringToCenter ? _rb.position : _initialReferencePoint + (_rb.position - initialPos);
            var force = (_posToHold - pointToHold) * _strength;
            
            Vector3 directionToWaypoint = _posToHold - _rb.position;
            float distanceToWaypoint = directionToWaypoint.magnitude;
        
            
            // What are the reprecussions?
            if (modifyVelocityDirectly)
            {
                _rb.velocity = force;
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
                    Vector3 brakingForce = -_rb.velocity.normalized * brakingStrength;
                    _rb.AddForce(brakingForce, ForceMode.Force);
                }
            }

            yield return new WaitForFixedUpdate();
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
