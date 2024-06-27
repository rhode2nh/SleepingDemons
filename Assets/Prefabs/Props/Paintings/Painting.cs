using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Painting : MonoBehaviour, IInteractable
{
    public List<List<string>> conversations;
    public List<string> lines;
    public int eyeCount;
    public bool isSecondPainting;
    public bool itemSpawned;
    public GameObject questItem;
    public Transform spawnPos;
    public Door door;

    public void ExecuteInteraction(GameObject other) {
        if (isSecondPainting) {
            SecondPainting();
        } else {
            FirstPainting();
        }
        FocusOn focusOn = other.gameObject.GetComponent<FocusOn>();
        focusOn.Focus(transform.position);
    }

    private void FirstPainting() {
        switch (eyeCount) {
            case 0: 
                DialogueBox.instance.StartConversation(lines[0]);
                break;
            case 1: 
                DialogueBox.instance.StartConversation(lines[1]);
                break;
            case 2:
                DialogueBox.instance.StartConversation(lines[2]);
                break;
            default:
                DialogueBox.instance.StartConversation(lines[3]);
                if (!itemSpawned) {
                    Invoke("SpawnQuestItem", 2.0f);
                }
                break;
        }
    }

    private void SecondPainting() {
        switch (eyeCount) {
            case 0: 
                DialogueBox.instance.StartConversation(lines[0]);
                break;
            default:
                DialogueBox.instance.StartConversation(lines[1]);
                if (!itemSpawned) {
                    Invoke("SpawnQuestItem", 2.0f);
                }
                break;
        }
        
    }

    private void SpawnQuestItem() {
        itemSpawned = true;
        Instantiate(questItem, spawnPos.position, transform.rotation);
    }

    public bool ExecuteOnRelease() { return false; }

    void OnCollisionEnter(Collision other) {
        if (isSecondPainting) {
            if (other.gameObject.tag == "Orb") {
                door.isLocked = false;
                eyeCount++;
                Destroy(other.gameObject);
            }
        } else {
            if (other.gameObject.tag == "ThrowableEye") {
                eyeCount++;
                Destroy(other.gameObject);
            }
        }
    }
}
