using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickupable
{
    public Item ItemSO { get; }
    public void DestroyItem();
    public void Pickup();
}
