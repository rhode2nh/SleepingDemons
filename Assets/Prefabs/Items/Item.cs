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
    [SerializeField] private GameObject _fpsItem;

    void Awake()
    {
        _GUID = Guid.NewGuid().ToString();
    }

    public string GUID { get => _GUID; }
    public Sprite Icon { get => _icon;}
    public int Cost { get => _cost;}
    public GameObject WorldItem { get => _worldItem; }
    public GameObject MarketItem { get => _marketItem; }
    public GameObject FpsItem { get => _fpsItem; }
}
