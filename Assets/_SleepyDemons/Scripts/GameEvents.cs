using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents instance;

    public event Action<string> onPickupMarketItem;

    void Awake()
    {
        instance = this;
    }

    public void PickupMarketItem(string worldItemInstanceID)
    {
        if (onPickupMarketItem != null)
        {
            onPickupMarketItem(worldItemInstanceID);
        }
    }
}
