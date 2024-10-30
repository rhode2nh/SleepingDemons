using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyLock : MonoBehaviour, ILock, IInteractable, IHittable
{
    [field: SerializeField] public bool IsLocked { get; private set; }
    [field: SerializeField] public bool IsBroken { get; private set; }
    [field: SerializeField] public Item Key { get; private set; }

    public void SwitchLock()
    {
        if (!IsBroken)
        {
            IsLocked = !IsLocked;
        }
    }

    public void BreakLock()
    {
        IsBroken = true;
        IsLocked = false;
    }

    public void ExecuteInteraction(GameObject other)
    {
        if (InventoryManager.instance.inventory.HasItem(Key.GUID))
        {
            SwitchLock();
        }
    }

    public bool ExecuteOnRelease()
    {
        return false;
    }

    public void TakeDamage(float damage, Vector3 force, Vector3 torque)
    {
        BreakLock();
    }
}
