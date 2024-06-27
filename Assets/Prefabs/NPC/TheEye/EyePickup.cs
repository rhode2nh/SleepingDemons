using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyePickup : MonoBehaviour, IInteractable
{
    public bool isOrb;
    public bool isFinalEye;
    public void ExecuteInteraction(GameObject other) {
        if (isOrb) {
            PickUpOrb();
        } else if (isFinalEye) {
            PickUpFinalEye();
        } else {
            PickUpEye();
        }
    }

    private void PickUpEye() {
        // PlayerManager.instance.hasItem = true;
        // // PlayerManager.instance.ShowFpsEye();
        // Destroy(gameObject);
    }

    private void PickUpOrb() {
        // PlayerManager.instance.hasItem = true;
        // PlayerManager.instance.ShowFpsOrb();
        // Destroy(gameObject);
    }

    private void PickUpFinalEye() {
        // PlayerManager.instance.hasItem = true;
        // PlayerManager.instance.hasFinalEye = true;
        // // PlayerManager.instance.ShowFpsFinalEye();
        // Destroy(gameObject);
    }
    public bool ExecuteOnRelease() { return false; }
}
