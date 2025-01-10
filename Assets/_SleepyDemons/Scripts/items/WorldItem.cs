using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldItem : Interactable, IPickupable
{
    [SerializeField] private Item _itemSO;

    public Item ItemSO { get => _itemSO; }

    public void DestroyItem()
    {
        Destroy(gameObject);
    }

    public override void ExecuteInteraction(GameObject other)
    {
        // if (PlayerManager.instance.isHolding)
        // {
            Pickup();
        // }
    }

    public void Pickup()
    {
        bool isAdded = InventoryManager.instance.inventory.Add(new Slot(ItemSO));
        if (isAdded) {
            MarketManager.instance.RemoveItemToSell(gameObject);
            InventoryManager.instance.UpdateInventoryGUI();
            DestroyItem();
        }
    }
}
