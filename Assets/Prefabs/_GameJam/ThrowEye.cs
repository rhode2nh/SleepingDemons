using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowEye : MonoBehaviour
{
    public GameObject throwableEye;
    public GameObject fpsItem;
    public float force;

    public void Throw() {
        // if (PlayerManager.instance.hasItem) {
        //     var instantiatedEye = Instantiate(throwableEye, transform.position, transform.rotation);
        //     var rb = instantiatedEye.GetComponent<Rigidbody>();
        //     rb.AddForce(transform.forward * force, ForceMode.Impulse);
        //     rb.AddTorque(transform.forward * force);
        //     PlayerManager.instance.hasItem = false;
        //     fpsItem.SetActive(false);
        //     // PlayerManager.instance.HideFpsEye();
        // }
    }
}
