using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MarketStats : MonoBehaviour
{
    [SerializeField] private Slot currentSlot;
    [SerializeField] private TMP_Text cost;
    [SerializeField] private TMP_Text damage;

    public void UpdateSlot(Slot slot)
    {
        currentSlot = slot;
        UpdateText();
    }

    private void UpdateText()
    {
        if (currentSlot.Item == DatabaseManager.instance.GetEmptyItem())
        {
            cost.SetText(string.Empty);
            damage.SetText(string.Empty);
        }
        else
        {
            cost.SetText("Cost: " + currentSlot.Item.Cost);
            damage.SetText("dmg: " + "0");
        }
    }
}
