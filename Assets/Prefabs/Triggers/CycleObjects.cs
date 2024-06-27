using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CycleObjects : MonoBehaviour
{
    public GameObject objectsToCycle;
    int index;
    int length;

    void Start() {
        index = 0;
        length = objectsToCycle.transform.childCount;
        objectsToCycle.transform.GetChild(index).gameObject.SetActive(true);
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            Cycle();
        }
    }

    void Cycle() {
        objectsToCycle.transform.GetChild(index).gameObject.SetActive(false);
        ++index;
        index = index % length;
        objectsToCycle.transform.GetChild(index).gameObject.SetActive(true);
    }
}
