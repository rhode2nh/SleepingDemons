using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Slot : ISlot
{
    [SerializeField] [ReadOnly] private string _name;
    [SerializeField] [ReadOnly] private string _GUID;
    [SerializeField] [ReadOnly] private Item _item;

    public Slot(Item item)
    {
        _GUID = Guid.NewGuid().ToString();
        _item = item;
        _name = item.name;
    }
    
    public Slot(Slot slot)
    {
        _GUID = slot.GUID;
        _item = slot.Item;
        _name = slot.Name;
    }

    public string Name { get => _name; }
    public string GUID { get => _GUID; }
    public Item Item { get => _item; }
    public bool IsEmpty { get => _item == DatabaseManager.instance.GetEmptyItem(); }
}
