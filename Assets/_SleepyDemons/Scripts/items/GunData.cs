using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "gun", menuName = "Items/New Gun")]
public class GunData : Item
{
    [field: SerializeField] public GameObject FpsItem { get; private set; }

    public override void Use()
    {
        var instantiatedWeapon = Instantiate(FpsItem, PlayerManager.Instance.WeaponSpawnPos.transform);
        InventoryManager.instance.EquipWeapon(instantiatedWeapon);
    }
}
