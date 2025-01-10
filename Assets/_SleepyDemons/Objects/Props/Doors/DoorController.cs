using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class DoorController : Interactable
{
    [SerializeField] private PhysicsDoor physicsDoor;
    [SerializeField] private LockController lockController;
    
    public override void ExecuteInteraction(GameObject other)
    {
        if (IsLocked())
        {
            physicsDoor.Hold(other.GetComponent<Interact>()._inputRaycast.hit.point, PlayerManager.instance.isHolding);
        }
    }

    private bool IsLocked()
    {
        return lockController.IsLocked();
    }
}
