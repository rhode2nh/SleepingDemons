using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour, IInteractable
{
    public GameObject interactable;
    public AudioSource audioSource;

    public void ExecuteInteraction(GameObject other) {
        interactable.GetComponent<IInteractable>().ExecuteInteraction(other);
        audioSource.Play();
    }

    public bool ExecuteOnRelease() { return false; }
}
