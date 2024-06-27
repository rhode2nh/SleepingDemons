using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class TeleportPlayer : MonoBehaviour
{
    public int numLevels;
    public int yOffset;
    public float sphereRadius;
    public TeleportWithPlayer teleportWithPlayer;
    public TeleportBelowPlayer teleportBelowPlayer;
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            // var colliders = Physics.OverlapSphere(other.transform.position, sphereRadius);
            // if (colliders.Length > 0) {
            //     for (int i = 0; i < colliders.Length; i++) {
            //         var tag = colliders[i].tag;
            //         bool isInList = objectsToTeleport.Any(x => x.GetInstanceID() == other.gameObject.GetInstanceID());
            //         if (tag == "ThrowableEye" || tag == "Orb" || tag == "FinalEye" && !isInList) {
            //             // objectsToTeleport.Add(colliders[i].gameObject);
            //         }
            //     }
            // }
            other.gameObject.GetComponent<CharacterController>().enabled = false;
            Vector3 newPos = other.gameObject.transform.position;
            newPos.y += numLevels * yOffset;
            other.gameObject.transform.position = newPos;
            other.gameObject.GetComponent<CharacterController>().enabled = true;
            teleportBelowPlayer.objectsTeleportedWithPlayer = new List<GameObject>(teleportWithPlayer.objectsToTeleport);
            for (int i = 0; i < teleportWithPlayer.objectsToTeleport.Count; i++) {
                var rb = teleportWithPlayer.objectsToTeleport[i].GetComponentInParent<Rigidbody>();
                var objectNewPos = rb.transform.position;
                objectNewPos.y += numLevels * yOffset;
                rb.position = objectNewPos;

                // var objectNewPos = teleportWithPlayer.objectsToTeleport[i].transform.position;
                // objectNewPos.y += numLevels * yOffset;
                // teleportWithPlayer.objectsToTeleport[i].transform.position = objectNewPos;
            }
        }
    }
}
