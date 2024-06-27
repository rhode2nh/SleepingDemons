using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatArea : MonoBehaviour
{
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.GetComponentInParent<IInteractable>() != null) {
            var rb = other.gameObject.GetComponentInParent<Rigidbody>();
            rb.drag = 2.0f;
        }
    }
    
    void OnTriggerExit(Collider other) {
        if (other.gameObject.GetComponentInParent<IInteractable>() != null) {
            var rb = other.gameObject.GetComponentInParent<Rigidbody>();
            rb.drag = 0.5f;
        }
    }
}
