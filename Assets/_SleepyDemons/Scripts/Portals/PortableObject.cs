using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PortableObject : MonoBehaviour
{
    private GameObject _cloneObject;

    private int _inPortalCount = 0;
    
    protected Portal InPortal;
    protected Portal OutPortal;

    private Rigidbody _rigidbody;
    [SerializeField] protected Collider _collider;

    protected static readonly Quaternion HalfTurn = Quaternion.Euler(0.0f, 180.0f, 0.0f);

    protected virtual void Awake()
    {
        _cloneObject = new GameObject();
        _cloneObject.SetActive(false);
        var meshFilter = _cloneObject.AddComponent<MeshFilter>();
        var meshRenderer = _cloneObject.AddComponent<MeshRenderer>();

        meshFilter.mesh = GetComponent<MeshFilter>().mesh;
        meshRenderer.materials = GetComponent<MeshRenderer>().materials;
        _cloneObject.transform.localScale = transform.localScale;

        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    private void LateUpdate()
    {
        if(InPortal == null || OutPortal == null)
        {
            return;
        }

        if(_cloneObject.activeSelf && InPortal.IsPlaced() && OutPortal.IsPlaced())
        {
            var inTransform = InPortal.Center;
            var outTransform = OutPortal.Center;

            // Update position of clone.
            Vector3 relativePos = inTransform.InverseTransformPoint(transform.position);
            relativePos = HalfTurn * relativePos;
            _cloneObject.transform.position = outTransform.TransformPoint(relativePos);

            // Update rotation of clone.
            Quaternion relativeRot = Quaternion.Inverse(inTransform.rotation) * transform.rotation;
            relativeRot = HalfTurn * relativeRot;
            _cloneObject.transform.rotation = outTransform.rotation * relativeRot;
        }
        else
        {
            _cloneObject.transform.position = new Vector3(-1000.0f, 1000.0f, -1000.0f);
        }
    }

    public virtual void SetIsInPortal(Portal inPortal, Portal outPortal, List<Collider> wallColliders)
    {
        this.InPortal = inPortal;
        this.OutPortal = outPortal;

        foreach (var wallCollider in wallColliders)
        {
            Physics.IgnoreCollision(_collider, wallCollider);
        }

        _cloneObject.SetActive(true);

        ++_inPortalCount;
    }

    public virtual void Warp()
    {
        var inTransform = InPortal.Center;
        var outTransform = OutPortal.Center;

        // Update position of object.
        Vector3 relativePos = inTransform.InverseTransformPoint(transform.position);
        relativePos = HalfTurn * relativePos;
        transform.position = outTransform.TransformPoint(relativePos);

        // Update rotation of object.
        Quaternion relativeRot = Quaternion.Inverse(inTransform.rotation) * transform.rotation;
        relativeRot = HalfTurn * relativeRot;
        transform.rotation = outTransform.rotation * relativeRot;

        // Update velocity of rigidbody.
        Vector3 relativeVel = inTransform.InverseTransformDirection(_rigidbody.linearVelocity);
        relativeVel = HalfTurn * relativeVel;
        
        // Update torque of rigidbody
        Vector3 relativeTorque = inTransform.InverseTransformDirection(_rigidbody.angularVelocity);
        relativeTorque = HalfTurn * relativeTorque;
        
        Physics.SyncTransforms();
        _rigidbody.linearVelocity = outTransform.TransformDirection(relativeVel);
        _rigidbody.angularVelocity = outTransform.TransformDirection(relativeTorque);

        // Swap portal references.
        (InPortal, OutPortal) = (OutPortal, InPortal);
    }

    public virtual void ExitPortal(List<Collider> wallColliders)
    {
        foreach (var wallCollider in wallColliders)
        {
            Physics.IgnoreCollision(_collider, wallCollider, false);
        }
        --_inPortalCount;

        if (_inPortalCount == 0)
        {
            _cloneObject.SetActive(false);
        }
    }
}
