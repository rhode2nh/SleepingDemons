using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public Inventory inventory;
    public Slot equippedWeapon;
    public IEquippable equippable;
    private GameObject _equippedWeaponGameobject;
    public int eyes;

    public event Action<int> onUpdateEyeGUI;
    public event Action onUpdateInventoryGUI;

    void Awake()
    {
        instance = this;
        eyes = 0;
    }

    void Start()
    {
        equippedWeapon = DatabaseManager.instance.GetEmptySlot();
    }

    public Slot Remove(int index)
    {
        Slot slot = inventory.Remove(index);
        if (!equippedWeapon.IsEmpty)
        {
            if (slot.GUID == equippedWeapon.GUID)
            {
                UnequipWeapon();
            }
        }
        return slot;
    }

    public Slot Remove(string GUID)
    {
        Slot slot = inventory.Remove(GUID);
        if (!equippedWeapon.IsEmpty)
        {
            if (slot.GUID == equippedWeapon.GUID)
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
        if (onUpdateEyeGUI != null)
        {
            onUpdateEyeGUI(eyes);
        }
    }

    public void UpdateInventoryGUI()
    {
        if (onUpdateInventoryGUI != null)
        {
            onUpdateInventoryGUI();
        }
    }
    
    public void EquipWeapon(int index)
    {
        if (_equippedWeaponGameobject != null)
        {
            UnequipWeapon();
        }
        equippedWeapon = inventory.Get(index);
        _equippedWeaponGameobject = Instantiate(equippedWeapon.Item.FpsItem, PlayerManager.instance.weaponSpawnPos.transform);
        equippable = _equippedWeaponGameobject.GetComponent<IEquippable>();
        _equippedWeaponGameobject.transform.localPosition = equippable.GetEquipPos();
    }

    public void UnequipWeapon()
    {
        equippedWeapon = DatabaseManager.instance.GetEmptySlot();
        equippable = null;
        Destroy(_equippedWeaponGameobject);
    }
}
