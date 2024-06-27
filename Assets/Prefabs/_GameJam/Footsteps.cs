using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    public float frequency;
    public float sprintFrequency;
    public bool isWalking;
    public bool isSprinting;
    public bool coroutineStarted;
    private AudioSource audioSource;
    private FirstPersonController firstPersonController;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        coroutineStarted = false;
        firstPersonController = GetComponentInParent<FirstPersonController>();
    }

    void Update() {
        if (isWalking && !coroutineStarted) {
            coroutineStarted = true;
            StartCoroutine(PlayFoosteps(frequency, sprintFrequency));
        }
    }

    public IEnumerator PlayFoosteps(float frequency, float sprintFrequency) {
        while (isWalking) {
            if (isSprinting) {
                yield return new WaitForSeconds(sprintFrequency);
            } else {
                yield return new WaitForSeconds(frequency);
            }
            if (!isWalking || !firstPersonController.Grounded) {
                break;
            }
            audioSource.Play();
        }
        coroutineStarted = false;
    }
}