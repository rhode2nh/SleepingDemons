using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Currency : MonoBehaviour, IInteractable, ICurrency, IPickupable
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

    public void ExecuteInteraction(GameObject other)
    {
        Pickup();
    }

    public bool IsBag()
    {
        return _isBag;
    }

    public bool ExecuteOnRelease() { return false; }

    public void Pickup()
    {
        bool isAdded = AddToCurrencyStash();
        if (isAdded)
        {
            DestroyItem();
        }
    }
}
