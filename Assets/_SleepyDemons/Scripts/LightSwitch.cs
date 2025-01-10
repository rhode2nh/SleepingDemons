using UnityEngine;

public class LightSwitch : Interactable
{
    public Light[] lightSources;
    public AudioSource audioSource;
    public bool isOn;

    public override void ExecuteInteraction(GameObject other) {
        for (int i = 0; i < lightSources.Length; i++) {
            lightSources[i].enabled = !lightSources[i].enabled;
            isOn = lightSources[i].enabled;
        }

        // audioSource.Play();
    }
}
