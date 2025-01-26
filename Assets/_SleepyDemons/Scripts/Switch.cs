using UnityEngine;

public class Switch : Interactable
{
    public GameObject interactable;
    public AudioSource audioSource;

    public override void ExecuteInteraction(GameObject other) {
        interactable.GetComponent<IInteractable>().ExecuteInteraction(other);
        audioSource.Play();
    }
}
