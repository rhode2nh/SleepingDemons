using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using StarterAssets;
using UnityEngine;

public class Falling : MonoBehaviour
{
    private FirstPersonController firstPersonController;
    public AudioSource fallingSound;
    public bool isPlaying;
    public float delay;
    public float velocityThreshold;
    public float curVolume;
    public float pitchFactor;
    // Start is called before the first frame update
    void Start()
    {
        firstPersonController = GetComponentInParent<FirstPersonController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!firstPersonController.Grounded) {
            if (!isPlaying) {
                fallingSound.Play();
                isPlaying = true;
            }
            if (Mathf.Abs(firstPersonController.VerticalVelocity) >= velocityThreshold) {
                curVolume = (Mathf.Abs(firstPersonController.VerticalVelocity) - velocityThreshold) / (firstPersonController.TerminalVelocity - velocityThreshold);
                fallingSound.volume = curVolume;
                fallingSound.pitch = 1.0f + (curVolume * pitchFactor);
            }
        } else {
            StartCoroutine(FadeOut());
        } 
    }

    IEnumerator FadeOut() {
        while (fallingSound.volume > 0.01f) {
            fallingSound.volume -= 0.01f;
            yield return new WaitForSeconds(delay);
        }
        fallingSound.Stop();
        isPlaying = false;
        fallingSound.volume = 0.0f;
    }
}
