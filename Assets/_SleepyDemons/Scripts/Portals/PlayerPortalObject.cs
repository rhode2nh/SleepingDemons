using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPortalObject : PortableObject
{
    private CharacterController _characterController;
    private RecursiveApartment _recursiveApartment;

    // private PortalLightObject _portalLightObject;
    
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        
        _characterController = GetComponent<CharacterController>();
        // _portalLightObject = GetComponentInChildren<PortalLightObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var apt = other.gameObject.GetComponent<RecursiveApartment>();
        if (apt != null)
        {
            _recursiveApartment = apt;
        }
    }

    public override void SetIsInPortal(Portal inPortal, Portal outPortal, List<Collider> wallColliders)
    {
        base.SetIsInPortal(inPortal, outPortal, wallColliders);
        
        // _portalLightObject.SetPortals(InPortal, outPortal);
    }

    public override void ExitPortal(List<Collider> wallColliders)
    {
        base.ExitPortal(wallColliders);
        
        // _portalLightObject.ExitPortals();
    }

    private void ManualTriggerExit(Collider other)
    {
        if (_recursiveApartment != null)
        {
            _recursiveApartment.OnTriggerExit(other);
        }
    }

    public override void Warp()
    {
        var inTransform = InPortal.Center;
        var outTransform = OutPortal.Center;

        // Update position of object.
        _characterController.enabled = false;
        Vector3 relativePos = inTransform.InverseTransformPoint(transform.position);
        relativePos = HalfTurn * relativePos;
        transform.position = outTransform.TransformPoint(relativePos);
        _characterController.enabled = true;
        InPortal._portalObjects.Remove(this);
        ManualTriggerExit(_characterController);

        // Update rotation of object.
        Quaternion relativeRot = Quaternion.Inverse(inTransform.rotation) * transform.rotation;
        relativeRot = HalfTurn * relativeRot;
        transform.rotation = Quaternion.Euler(0, (outTransform.rotation * relativeRot).eulerAngles.y, 0);
        
        // Update velocity of rigidbody.
        Physics.SyncTransforms();
        Vector3 relativeVel = inTransform.InverseTransformDirection(_characterController.velocity);
        relativeVel = HalfTurn * relativeVel;
        _characterController.Move(outTransform.TransformDirection(relativeVel) * Time.deltaTime);

        // Swap portal references.
        (InPortal, OutPortal) = (OutPortal, InPortal);
    }
}
