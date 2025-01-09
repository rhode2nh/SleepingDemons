using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellItem : MonoBehaviour
{
    [SerializeField] private List<GameObject> itemsToSell;
    [SerializeField] private float timeToSell;
    [SerializeField] private float totalEyeCount;
    [SerializeField] private GameObject currency;

    void Awake()
    {
        itemsToSell = new List<GameObject>();
    }

    void Start()
    {
        MarketManager.instance.onRemoveItemToSell += RemoveFromMarket;
    }

    void RemoveFromMarket(GameObject worldItem)
    {
        if (itemsToSell.Contains(worldItem))
        {
            itemsToSell.Remove(worldItem);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        StopAllCoroutines();
        if (other.gameObject.GetComponent<WorldItem>() != null)
        {
            itemsToSell.Add(other.gameObject);
            StartCoroutine(SellItems());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<WorldItem>() != null)
        {
            RemoveFromMarket(other.gameObject);
        }
    }

    private IEnumerator SellItems()
    {
        yield return new WaitForSeconds(timeToSell);

        for (int i = 0; i < itemsToSell.Count; i++)
        {
            totalEyeCount += itemsToSell[i].GetComponent<WorldItem>().ItemSO.Cost;
            Destroy(itemsToSell[i].gameObject);
        }

        for (int i = 0; i < totalEyeCount; i++)
        {
            Instantiate(currency, transform.position, transform.rotation);
        }

        itemsToSell.Clear();
        totalEyeCount = 0;
    }
}
