using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Currency : Interactable, ICurrency, IPickupable
{
    [SerializeField] public int amount; 
    [SerializeField] private Item _itemSO;
    [SerializeField] private bool _isBag;
    public Item ItemSO => _itemSO;

    public bool AddToCurrencyStash()
    {
        InventoryManager.instance.IncreaseEyeCount(amount);
        return true;
    }

    public void DestroyItem()
    {
        Destroy(gameObject);
    }

    public override void ExecuteInteraction(GameObject other)
    {
        Pickup();
    }

    public bool IsBag()
    {
        return _isBag;
    }

    public void Pickup()
    {
        bool isAdded = AddToCurrencyStash();
        if (isAdded)
        {
            DestroyItem();
        }
    }
}
