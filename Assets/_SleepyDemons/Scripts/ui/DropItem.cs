using UnityEngine;

[CreateAssetMenu(fileName = "SO_DropItem", menuName = "Dropdown Options/New Drop Item Option")]
public class DropItem : DropdownOption
{
    public override void Process(Slot slot)
    {
        Slot removedSlot = InventoryManager.instance.Remove(slot.GUID);
        InventoryManager.instance.UpdateInventoryGUI();
        Instantiate(removedSlot.Item.WorldItem, PlayerManager.instance.dropItemSpawnPos.position, Quaternion.identity);
    }
}
