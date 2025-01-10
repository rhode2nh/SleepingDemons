using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketOffer : Interactable, IPickupable
{
    [SerializeField] private Item _itemSO;
    [SerializeField] private float _rotationSpeed;   

    public Item ItemSO { get => _itemSO; set => _itemSO = value; }
    public MarketArmEvents marketArmEvents;

    void Update()
    {
        gameObject.transform.Rotate(Vector3.up * (_rotationSpeed * Time.deltaTime));
    }

    public void DestroyItem()
    {
        if (marketArmEvents != null)
        {
            marketArmEvents.StopOffering();
        }
        Destroy(gameObject);
    }

    public override void ExecuteInteraction(GameObject other)
    {
        Pickup();
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
