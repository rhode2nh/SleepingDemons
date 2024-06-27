using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TeleportBelowPlayer : MonoBehaviour
{
    public List<GameObject> objectsTeleportedWithPlayer;
    public List<GameObject> objectsToTeleport;
    public float yOffset;
    public bool playerInTrigger;

    void OnTriggerEnter(Collider other) {
        var tag = other.gameObject.tag;
        bool isInList = objectsTeleportedWithPlayer.Any(x => x.GetInstanceID() == other.gameObject.GetInstanceID());
        if ((tag == "ThrowableEye" || tag == "Orb" || tag == "FinalEye") && !isInList) {
            // objectsTeleportedWithPlayer.Add(other.gameObject);
            // var rb = other.GetComponentInParent<Rigidbody>();
            // var objectNewPos = rb.transform.position;
            // objectNewPos.y -= yOffset;
            // // teleportWithPlayer.objectsToTeleport[i].SetActive(false);
            // rb.position = objectNewPos;
            objectsToTeleport.Add(other.gameObject);
        // } else if (tag == "ThrowableEye" || tag == "Orb" || tag == "FinalEye") {
        //     objects
        } else if (tag == "Player") {
            playerInTrigger = true;
            for (int i = 0; i < objectsToTeleport.Count; i++) {
                var rb = objectsToTeleport[i].GetComponentInParent<Rigidbody>();
                var objectNewPos = rb.transform.position;
                objectNewPos.y -= yOffset;
                rb.position = objectNewPos;
            }
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            playerInTrigger = false;
            objectsToTeleport = new List<GameObject>(objectsTeleportedWithPlayer);
            // objectsTeleportedWithPlayer.Clear();
        } else if (tag == "ThrowableEye" || tag == "Orb" || tag == "FinalEye") {
            bool isInList = objectsToTeleport.Any(x => x.GetInstanceID() == other.gameObject.GetInstanceID());
            if (isInList) {
                objectsToTeleport.Remove(other.gameObject);
            }
        }
    }
}
