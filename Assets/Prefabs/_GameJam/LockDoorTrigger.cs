using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockDoorTrigger : MonoBehaviour
{
    public Door door;
    void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            if (door.isOpen) {
                Vector3 doorRelative = transform.InverseTransformPoint(other.gameObject.transform.position);
                if (doorRelative.x < 0.0f) {
                    door.CloseDoor();
                    door.isLocked = true;
                }
            }
        }
    }
}
