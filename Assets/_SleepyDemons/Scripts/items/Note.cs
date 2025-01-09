using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour, IPickupable, IInteractable
{
    [field: SerializeField] public Item ItemSO { get; private set; }
    public void DestroyItem()
    {
        Destroy(gameObject);
    }

    public void Pickup()
    {
        bool isAdded = InventoryManager.instance.noteInventory.Add(new Slot(ItemSO));
        if (!isAdded) return;
        
        MarketManager.instance.RemoveItemToSell(gameObject);
        InventoryManager.instance.UpdateInventoryGUI();
        DestroyItem();
    }

    public void ExecuteInteraction(GameObject other)
    {
        Pickup();
    }

    public bool ExecuteOnRelease()
    {
        return false;
    }
}
