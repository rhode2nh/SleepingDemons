using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IUIInventory
{
    void SetMarket(Market market);
    void OnPointerClickFunc(PointerEventData eventData, int index); 
}
