using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager instance;
    public ItemDatabase itemDatabase;

    [SerializeField] private Item emptyItem;
    private Slot emptySlot;

    void Awake()
    {
        instance = this;
        emptySlot = new Slot(emptyItem);
    }

    public Item GetEmptyItem()
    {
        return emptyItem;
    }

    public Slot GetEmptySlot()
    {
        return emptySlot;
    }

    public Item GetRandomItem()
    {
        var index = Random.Range(0, itemDatabase.items.Count);
        return itemDatabase.GetItem(index);
    }
}
