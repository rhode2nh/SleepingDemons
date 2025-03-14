using UnityEngine;

[CreateAssetMenu(fileName = "SO_Dropdown_UnequipItem", menuName = "Dropdown Options/New Unequip Item Option")]
public class UnequipItem : DropdownOption
{
    public override void Process(Slot slot)
    {
        InventoryManager.instance.UnequipWeapon();
    }
}
