using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsEye : MonoBehaviour
{
    void Awake() {
        var painting = FindObjectOfType<Painting>(true);
        if (painting != null) {
            GetComponentInChildren<LookAt>().objectToLookAt = painting.gameObject;
        }
    }
}
