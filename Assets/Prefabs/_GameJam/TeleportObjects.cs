using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TeleportObjects : MonoBehaviour
{
    public float yOffset;
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.GetComponentInParent<IInteractable>() != null) {
            var rb = other.gameObject.GetComponentInParent<Rigidbody>();
            var newPos = rb.transform.position;
            newPos.y += yOffset;
            other.gameObject.SetActive(false);
            rb.position = newPos;
            other.gameObject.SetActive(true);
        }
    }
}
