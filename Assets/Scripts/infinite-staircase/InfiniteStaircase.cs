using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class InfiniteStaircase : MonoBehaviour
{
    [SerializeField] private float _yOffset;
    [SerializeField] private float _floorsToTeleport;

    [SerializeField] private List<GameObject> itemsToTeleport;
    [SerializeField] private float maxDistanceCheck;

    public void AddTeleportItem(GameObject item)
    {
        itemsToTeleport.Add(item);
    }
    
    public void RemoveTeleportItem(GameObject item)
    {
        bool hasRemoved = itemsToTeleport.Remove(item);
        if (hasRemoved)
        {
            Destroy(item);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<FirstPersonController>() != null)
        {
            var playerPosition = other.transform.position;
            
            // Teleport Items
            // foreach (var item in itemsToTeleport)
            // {
            //     item.transform.position += new Vector3(0.0f, _yOffset * _floorsToTeleport, 0.0f);
            // }
            
            // Teleport Player
            other.GetComponent<CharacterController>().enabled = false;
            other.transform.position += new Vector3(0.0f, _yOffset * _floorsToTeleport, 0.0f);
            other.GetComponent<CharacterController>().enabled = true;
            var currentItems = new List<GameObject>(itemsToTeleport);

            // Remove newly spawned items
            foreach (var item in currentItems)
            {
                if (Vector3.Distance(other.transform.position, item.transform.position) > maxDistanceCheck) continue;
                
                // item.transform.position += new Vector3(0.0f, _yOffset * _floorsToTeleport, 0.0f);
                // itemsThatCanTeleport.Add(item);
                RemoveTeleportItem(item);
            }
            
            foreach (var item in currentItems)
            {
                if (Vector3.Distance(playerPosition, item.transform.position) > maxDistanceCheck) continue;
                
                // item.transform.position += new Vector3(0.0f, _yOffset * _floorsToTeleport, 0.0f);
                // itemsThatCanTeleport.Add(item);
                item.transform.position += new Vector3(0.0f, _yOffset * _floorsToTeleport, 0.0f);
            }
        }
    }
}
