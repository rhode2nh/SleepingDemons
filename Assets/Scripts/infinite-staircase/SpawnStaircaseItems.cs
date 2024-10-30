using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class SpawnStaircaseItems : MonoBehaviour
{
    [SerializeField] private float maxSpawnDistance;
    [SerializeField] private bool canSpawnItems;
    [SerializeField] private float spawnRate;
    [SerializeField] private GameObject itemToSpawn;
    [SerializeField] private GameObject spawnPosition;
    [SerializeField] private InfiniteStaircase infiniteStaircase;

    IEnumerator Start()
    {
        while (true)
        {
            if (canSpawnItems)
            {
                var instantiatedItem = Instantiate(itemToSpawn, spawnPosition.transform.position, new Quaternion());
                infiniteStaircase.AddTeleportItem(instantiatedItem);
            }

            yield return new WaitForSeconds(spawnRate);
        }
    }
    
    void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<FirstPersonController>() != null)
        {
            if (Vector3.Distance(other.transform.position, transform.position) < maxSpawnDistance)
            {
                canSpawnItems = false;
            }
            else
            {
                canSpawnItems = true;
            }
        }
    }
}
