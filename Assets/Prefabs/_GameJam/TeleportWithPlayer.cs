using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TeleportWithPlayer : MonoBehaviour
{
    public List<GameObject> objectsToTeleport;

    void OnTriggerEnter(Collider other) {
        var tag = other.gameObject.tag;
        bool isInList = objectsToTeleport.Any(x => x.GetInstanceID() == other.gameObject.GetInstanceID());
        if (tag == "ThrowableEye" || tag == "Orb" || tag == "FinalEye" && !isInList) {
            objectsToTeleport.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other) {
        var tag = other.gameObject.tag;
        bool isInList = objectsToTeleport.Any(x => x.GetInstanceID() == other.gameObject.GetInstanceID());
        if (tag == "ThrowableEye" || tag == "Orb" || tag == "FinalEye" && isInList) {
            objectsToTeleport.Remove(other.gameObject);
        }

    }
}
