using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropEyes : MonoBehaviour
{
    [SerializeField] private GameObject _burlapSack;

    public void DropSack(int amount)
    {
        int curEyeCount = InventoryManager.instance.eyes;
        if (curEyeCount <= 0)
        {
            return;
        }

        GameObject instantiatedSack = Instantiate(_burlapSack, PlayerManager.instance.dropItemSpawnPos.position, Quaternion.identity);

        if (curEyeCount - amount < 0)
        {
            instantiatedSack.GetComponent<Currency>().amount = curEyeCount;
            InventoryManager.instance.IncreaseEyeCount(-InventoryManager.instance.eyes);
        }
        else
        {
            instantiatedSack.GetComponent<Currency>().amount = amount;
            InventoryManager.instance.IncreaseEyeCount(-amount);
        }
    }
}
