using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public bool openRelativeToPlayer;
    public bool isOpen;
    public bool isLocked;
    public Animator animator;
    public AudioSource audioSource;
    public AudioClip[] doorSounds;
    public float audioDelay;

    public void ExecuteInteraction(GameObject other) {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f) {
            if (isLocked) {
                NudgeDoor();
            } else if (!isOpen) {
                if (openRelativeToPlayer) {
                    OpenRelativeToPlayer(other.transform.position);
                } else {
                    OpenDoor();
                }
            } else {
                CloseDoor();
            }
        }
    }

    private void playAudio() {
        audioSource.Play();
    }

    public bool ExecuteOnRelease() { return false; }

    private void NudgeDoor() {
        animator.SetBool("IsAttemptingToOpen", true);
        audioSource.clip = doorSounds[2];
        playAudio();
    }
    private void OpenRelativeToPlayer(Vector3 playerPos) {
        Vector3 doorRelative = transform.InverseTransformPoint(playerPos);
        if (doorRelative.x > 0.0f) {
            OpenDoor();
        } else {
            OpenDoorOtherSide();
        }
    }

    private void OpenDoor() {
        isOpen = true;
        animator.SetBool("IsOpening", isOpen);
        animator.SetBool("IsClosing", false);
        animator.SetBool("HasClosed", false);
        audioSource.clip = doorSounds[0];
        playAudio();
    }

    private void OpenDoorOtherSide() {
        isOpen = true;
        animator.SetBool("IsOpeningOtherSide", isOpen);
        animator.SetBool("IsClosing", false);
        animator.SetBool("HasClosed", false);
        audioSource.clip = doorSounds[0];
        playAudio();
    }

    public void CloseDoor() {
        isOpen = false;
        animator.SetBool("IsClosing", true);
        animator.SetBool("IsOpening", isOpen);
        animator.SetBool("IsOpeningOtherSide", isOpen);
        audioSource.clip = doorSounds[1];
        Invoke("playAudio", audioDelay);
    }

    private void HasClosed() {
        animator.SetBool("IsAttemptingToOpen", false);
        animator.SetBool("HasClosed", true);
    }

    private void ResetBools() {
        animator.SetBool("HasClosed", false);
    }
}
