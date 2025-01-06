using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class Market : MonoBehaviour, IMarket, IInteractable
{
    [SerializeField] private Inventory _marketInventory;
    [SerializeField] [ReadOnly] private int _index;
    [SerializeField] private GameObject _itemSpawnPos;
    
    private MarketUI _marketUI;

    void Start()
    {
        _marketUI = GameObject.FindWithTag("MarketUI").GetComponent<MarketUI>();
        _marketUI.gameObject.SetActive(false);
    }

    public void OpenMarket()
    {
        for (int i = 0; i < _marketInventory.MaxLength; i++)
        {
            _marketInventory.Add(new Slot(DatabaseManager.instance.GetRandomItem()));
        }
        _marketUI.SetMarket(this);
    }

    public void CloseMarket()
    {
        _marketUI.ClosePanel();
        _marketInventory.Clear();
    }

    public void NextItem()
    {
        // _index = (_index + 1) % _marketInventory.MaxLength;
    }

    public void PreviousItem()
    {
        // _index = (_index - 1) % _marketInventory.MaxLength;
    }

    public void BuyItem(int index)
    {
        Instantiate(_marketInventory.Get(index).Item.WorldItem, _itemSpawnPos.transform.position, _itemSpawnPos.transform.rotation);
        Slot slot = _marketInventory.Remove(index);
        InventoryManager.instance.IncreaseEyeCount(-slot.Item.Cost);
        InventoryManager.instance.UpdateEyeGUI();
    }

    public void SellItem()
    {
        throw new System.NotImplementedException();
    }

    public void ExecuteInteraction(GameObject other)
    {
        OpenMarket();
    }

    public bool ExecuteOnRelease()
    {
        return false;
    }

    public Inventory GetMarketInventory()
    {
        return _marketInventory;
    }
}
