using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour, IInventory
{
    [SerializeField] private List<Slot> _items;
    [SerializeField] private int _maxLength;

    void Awake()
    {
        _items = new List<Slot>();
    }

    void Start()
    {
        for (int i = 0; i < _maxLength; i++)
        {
            _items.Add(DatabaseManager.instance.GetEmptySlot());
        }
    }

    public bool Add(Slot slot)
    {
        for (var i = 0; i < _items.Count; i++)
        {
            if (_items[i].Item != DatabaseManager.instance.GetEmptyItem()) continue;
            _items[i] = new Slot(slot);
            return true;
        }

        Debug.LogError("Inventory is full. Cannot add item: " + slot.GUID);
        return false;
    }

    public Slot Get(int index)
    {
        try
        {
            return _items[index];
        }
        catch (Exception e)
        {
            Debug.LogError("Could not get item at index: " + index + "\nError: " + e.Message);
            return null;
        }
    }

    public Slot Remove(int index)
    {
        try
        {
            Slot item = new Slot(_items[index]);
            _items[index] = DatabaseManager.instance.GetEmptySlot();
            return item;
        }
        catch (Exception e)
        {
            Debug.LogError("Could not remove item at index: " + index + "\nError: " + e.Message);
            return null;
        }
    }

    public Slot Remove(string GUID)
    {
        try
        {
            Slot item = new Slot(_items.Where(x => x.GUID == GUID).First());
            int index = _items.FindIndex(x => x.GUID == GUID);
            _items[index] = DatabaseManager.instance.GetEmptySlot();
            return item;
        }
        catch (Exception e)
        {
            Debug.LogError("Could not remove item with GUID: " + GUID + "\nError: " + e.Message);
            return null;
        }
    }

    public void Clear()
    {
        for (int i = 0; i < _items.Count; i++)
        {
            _items[i] = DatabaseManager.instance.GetEmptySlot();
        }
    }

    public void Swap(int firstIndex, int secondIndex)
    {
        Slot firstItem = new Slot(_items[firstIndex]);
        _items[firstIndex] = _items[secondIndex];
        _items[secondIndex] = firstItem;
    }

    public int MaxLength { get => _maxLength; }

    public int Count { get => _items.Count(x => x.Item != DatabaseManager.instance.GetEmptyItem()); }
}
