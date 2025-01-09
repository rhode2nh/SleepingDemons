using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using UnityEngine;

public class LightSwitch : MonoBehaviour, IInteractable
{
    public Light[] lightSources;
    public AudioSource audioSource;
    public bool isOn;

    public void ExecuteInteraction(GameObject other) {
        for (int i = 0; i < lightSources.Length; i++) {
            lightSources[i].enabled = !lightSources[i].enabled;
            isOn = lightSources[i].enabled;
        }

        // audioSource.Play();
    }

    public bool ExecuteOnRelease() { return false; }
}
