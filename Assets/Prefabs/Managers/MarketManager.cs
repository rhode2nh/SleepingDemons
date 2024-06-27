using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketManager : MonoBehaviour
{
    public static MarketManager instance;

    void Awake()
    {
        instance = this;
    }

    public Action<GameObject> onRemoveItemToSell;

    public void RemoveItemToSell(GameObject worldItem)
    {
        if (onRemoveItemToSell != null)
        {
            onRemoveItemToSell(worldItem);
        }
    }
}
