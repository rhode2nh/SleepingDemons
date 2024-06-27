using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    public Vector3 positionToTeleport;
    public BoxCollider area;
    public float zOffset;
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "ThrowableEye" || other.gameObject.tag == "Orb" || other.gameObject.tag == "FinalEye") {
            var rb = other.gameObject.GetComponentInParent<Rigidbody>();
            // var newPos = rb.position;
            // newPos.z = transform.position.z - (area.size.z / 2.0f) - zOffset;
            // rb.position = newPos;

            var newPos = transform.forward * (area.size.z / 2.0f - zOffset);
            rb.position = newPos + rb.position;
            // rb.position = newPos;
        }
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        var newPos = positionToTeleport;
        newPos.z = transform.position.z - (area.size.z / 2.0f) - zOffset;
        Gizmos.DrawSphere(newPos, 0.1f);
    }
}
