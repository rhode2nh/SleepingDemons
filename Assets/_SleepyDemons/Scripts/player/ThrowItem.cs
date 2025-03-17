using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class ThrowItem : MonoBehaviour
{
    [SerializeField] private StarterAssetsInputs _input;
    [SerializeField] private GameObject throwPos;

    void Update()
    {
        if (_input.ThrowItem && !InventoryManager.instance.selectedSlot.IsEmpty)
        {
            Slot slot = InventoryManager.instance.Remove(InventoryManager.instance.selectedSlot.GUID);
            InventoryManager.instance.UpdateInventoryGUI();
            Instantiate(slot.Item.WorldItem, PlayerManager.Instance.DropItemSpawnPos.position, Quaternion.identity);
        }
    }
}
