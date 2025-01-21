using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holdable : Interactable, IHoldable
{
    [SerializeField] protected bool _isHolding;
    [SerializeField] private float _strength;
    [SerializeField] private bool _bringToCenter;
    [SerializeField] private Rigidbody rb;
    private Vector3 _initialReferencePoint;
    private Vector3 _relativePointToHold;

     void Start() {
        rb = GetComponent<Rigidbody>();
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
        var initialPos = rb.position;
        _initialReferencePoint = Camera.main.transform.position + Camera.main.transform.forward * relativeDistance;
        // rb.angularDrag = 3f;
        while (PlayerManager.instance.isHolding) {
            var posToHold = Camera.main.transform.position + (Camera.main.transform.forward * relativeDistance);
            if (_bringToCenter) {
                // What are the reprecussions?
                // rb.AddForce((posToHold - rb.position) * _strength);
                rb.velocity = (posToHold - rb.position) * _strength;
            } else {
                _relativePointToHold = _initialReferencePoint + (rb.position - initialPos);
                // rb.AddForce((posToHold - _relativePointToHold) * _strength);
                rb.velocity = (posToHold - _relativePointToHold) * _strength;
            }
            yield return new WaitForFixedUpdate();
        }
        // rb.angularDrag = 0.5f;
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
