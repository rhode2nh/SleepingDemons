using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holdable : Interactable, IHoldable
{
    [SerializeField] bool _isHolding;
    [SerializeField] private float _strength;
    [SerializeField] private bool _bringToCenter;
    private Vector3 _initialReferencePoint;
    private Vector3 _relativePointToHold;
    private Rigidbody _rb;

    void Start() {
        _rb = GetComponentInParent<Rigidbody>();
        _initialReferencePoint = new Vector3();
        _relativePointToHold = new Vector3();
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
        var initialPos = _rb.position;
        _initialReferencePoint = Camera.main.transform.position + Camera.main.transform.forward * relativeDistance;
        _rb.angularDrag = 3f;
        while (PlayerManager.instance.isHolding) {
            var posToHold = Camera.main.transform.position + (Camera.main.transform.forward * relativeDistance);
            if (_bringToCenter) {
                // What are the reprecussions?
                // _rb.AddForce((posToHold - _rb.position) * _strength);
                _rb.velocity = (posToHold - _rb.position) * _strength;
            } else {
                _relativePointToHold = _initialReferencePoint + (_rb.position - initialPos);
                // _rb.AddForce((posToHold - _relativePointToHold) * _strength);
                _rb.velocity = (posToHold - _relativePointToHold) * _strength;
            }
            yield return new WaitForFixedUpdate();
        }
        _rb.angularDrag = 0.5f;
    }

    void OnDrawGizmos() {
        if (_isHolding) {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(_relativePointToHold, 0.1f);
        }
    }

    public override void ExecuteInteraction(GameObject other)
    {
        // TODO: Is there some way to get around using the global variable?
        Hold(other.gameObject.GetComponent<Interact>()._inputRaycast.hit.point, PlayerManager.instance.isHolding);
    }
}
