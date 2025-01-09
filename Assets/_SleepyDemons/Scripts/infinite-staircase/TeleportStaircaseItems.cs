using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportStaircaseItems : MonoBehaviour
{
    [SerializeField] private float _yOffset;
    [SerializeField] private float _floorsToTeleport;
    [SerializeField] private GameObject teleportPosition;
    [SerializeField] private InfiniteStaircase infiniteStaircase;
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Rigidbody>() != null)
        {
            // var oldPos = other.transform.position;
            // other.transform.position = new Vector3(oldPos.x, teleportPosition.transform.position.y, oldPos.z);
            
            infiniteStaircase.RemoveTeleportItem(other.gameObject);
        }
    }
}
