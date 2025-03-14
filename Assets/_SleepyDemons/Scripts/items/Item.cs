using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Item", menuName = "Items/New Item")]
public class Item : ScriptableObject, IItem
{
    [SerializeField] [ReadOnly] private string _GUID;
    [SerializeField] private Sprite _icon;
    [SerializeField] private int _cost;
    [SerializeField] private GameObject _worldItem;
    [SerializeField] private GameObject _marketItem;

    [SerializeField] private List<DropdownOption> _options;
    // [SerializeField] private GameObject _fpsItem;

    void Awake()
    {
        _GUID = Guid.NewGuid().ToString();
        _options = new List<DropdownOption>();
    }

    public string GUID { get => _GUID; }
    public Sprite Icon { get => _icon;}
    public int Cost { get => _cost;}
    public GameObject WorldItem { get => _worldItem; }
    public GameObject MarketItem { get => _marketItem; }
    public List<DropdownOption> Options { get => _options; }

    // This method is called whenever an item in the inventory is used.
    public virtual void Use()
    {
        Debug.Log("Using item: " + _GUID);       
    }
}
