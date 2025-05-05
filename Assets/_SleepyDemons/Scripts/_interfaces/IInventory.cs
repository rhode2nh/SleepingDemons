using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventory
{
    Slot Get(int index);
    Slot Remove(int index);
    void Swap(int firstIndex, int secondIndex);
    bool Add(Slot item);
    int MaxLength { get; }
}
