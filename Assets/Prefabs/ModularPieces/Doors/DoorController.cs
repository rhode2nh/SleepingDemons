using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DoorController : MonoBehaviour, IInteractable
{
    [SerializeField] private PhysicsDoor _physicsDoor;
    [SerializeField] GameObject _lockParent;
    private List<ILock> _locks = new();
    
    // Start is called before the first frame update
    void Awake()
    {
        _physicsDoor = GetComponent<PhysicsDoor>();
        _locks = new List<ILock>(_lockParent.GetComponentsInChildren<ILock>());
    }
    
    public void ExecuteInteraction(GameObject other)
    {
        if (IsUnlocked())
        {
            _physicsDoor.Hold(other.GetComponent<Interact>()._inputRaycast.hit.point, PlayerManager.instance.isHolding);
        }
    }

    private bool IsUnlocked()
    {
        return _locks.All(llock => !llock.IsLocked);
    }

    public bool ExecuteOnRelease()
    {
        return true;
    }
}
