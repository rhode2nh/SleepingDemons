using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Database/Item Database", order = 1)]
public class ItemDatabase : ScriptableObject
{
    [SerializeField] public List<Item> items;

    public Item GetItem(string id)
    {
        return items.Where(x => x.GUID == id).FirstOrDefault();
    }

    public Item GetItem(int index)
    {
        return items[index];
    }
}

