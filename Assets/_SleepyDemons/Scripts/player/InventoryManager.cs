using System;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public Inventory inventory;
    public Inventory noteInventory;
    public Slot selectedSlot;
    public IEquippable Equippable;
    private GameObject _equippedWeaponGameobject;
    public int eyes;

    public event Action<int> OnUpdateEyeGUI;
    public event Action OnUpdateInventoryGUI;
    public event Action OnInitializeInventoryGUI;

    void Awake()
    {
        instance = this;
        eyes = 0;
    }

    void Start()
    {
        selectedSlot = DatabaseManager.instance.GetEmptySlot();
    }

    public Slot Remove(int index)
    {
        Slot slot = inventory.Remove(index);
        if (!selectedSlot.IsEmpty)
        {
            if (slot.GUID == selectedSlot.GUID)
            {
                UnequipWeapon();
            }
        }
        return slot;
    }

    public Slot Remove(string GUID)
    {
        Slot slot = inventory.Remove(GUID);
        if (!selectedSlot.IsEmpty)
        {
            if (slot.GUID == selectedSlot.GUID)
            {
                UnequipWeapon();
            }
        }
        return slot;
    }

    public void IncreaseEyeCount(int eyeCount)
    {
        eyes += eyeCount;
        UpdateEyeGUI();
    }

    public void UpdateEyeGUI()
    {
        if (OnUpdateEyeGUI != null)
        {
            OnUpdateEyeGUI(eyes);
        }
    }

    public void InitializeInventoryGUI()
    {
        OnInitializeInventoryGUI?.Invoke();
    }
    
    public void UpdateInventoryGUI()
    {
        OnUpdateInventoryGUI?.Invoke();
    }
    
    public void UseItem(string GUID)
    {
        selectedSlot = inventory.Get(GUID);
        selectedSlot.Item.Use();
    }
    
    public void UseItem(int index)
    {
        selectedSlot = inventory.Get(index);
        selectedSlot.Item.Use();
    }

    public void EquipWeapon(GameObject weapon)
    {
        if (_equippedWeaponGameobject != null)
        {
            UnequipWeapon();
        }
        _equippedWeaponGameobject = weapon;
        Equippable = _equippedWeaponGameobject.GetComponent<IEquippable>();
        _equippedWeaponGameobject.transform.localPosition = Equippable.GetEquipPos();
    }

    public void ClearSelectedSlot()
    {
        selectedSlot = DatabaseManager.instance.GetEmptySlot();
    }

    public void UnequipWeapon()
    {
        ClearSelectedSlot();
        Equippable = null;
        Destroy(_equippedWeaponGameobject);
    }
}
