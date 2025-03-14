using UnityEngine;

[CreateAssetMenu(fileName = "SO_UseItem", menuName = "Dropdown Options/New Use Item Option")]
public class UseItem : DropdownOption
{
    public override void Process(Slot slot)
    {
        InventoryManager.instance.UseItem(slot.GUID);
    }
}
