using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(HingeJoint))]
public class Door : Interactable
{
    [Header("References")]
    [SerializeField] private PhysicsDoor physicsDoor;
    [SerializeField] private LockController lockController;
    
    public override void ExecuteInteraction(GameObject other)
    {
        if (!IsLocked())
        {
            physicsDoor.Hold(other.GetComponent<Interact>()._inputRaycast.hit.point, PlayerManager.Instance.IsHolding);
        }
    }

    private bool IsLocked()
    {
        return lockController.IsLocked();
    }
}
