using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMarket
{
    public void OpenMarket();
    public void CloseMarket();
    public void NextItem();
    public void PreviousItem();
    public void BuyItem(int index);
    public void SellItem();
}
