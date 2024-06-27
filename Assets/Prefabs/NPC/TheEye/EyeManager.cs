using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class EyeManager : MonoBehaviour, IInteractable, IDialogue
{
    public List<List<string>> initialConversations;
    public List<List<string>> questConversations;
    public List<List<string>> finalConversations;
    public Door questDoor;
    public EndGame endGame;
    public int index = 0;
    public int conversationIndex = 0;

    void Start() {
        endGame.gameObject.GetComponent<Collider>().enabled = false;
        initialConversations = new List<List<string>>();
        questConversations = new List<List<string>>();
        finalConversations = new List<List<string>>();
        initialConversations.Add(new List<string> {"Joy! The door is finally opened!", "...", "Hmmm"});
        initialConversations.Add(new List<string> {
            "I've been imprisoned behind this door for ages, and now I finally get to see!",
            "...", 
            "Well...only partially.",
            "You see, I'm missing one of my eyes.",
            "Can you help me retrieve it?",
            "It's in the room behind the door to your right.",
            "The paintings should guide you towards the eye.",
            "Also...be wary of the other eyes.",
        });

        questConversations.Add(new List<string> {
            "Have you found the eye? If not, look to the paintings...",
        });

        finalConversations.Add(new List<string> {
            "You found the eye! Please give it to me.",
        });

        finalConversations.Add(new List<string> {
            "Thank you...you can go to bed now. Oh...and leave the door open",
        });
    }

    public void ExecuteInteraction(GameObject other) {
        switch (conversationIndex) {
            case 0:
                InitialConversation(other);
                break;
            case 1:
                QuestConversation(other);
                break;
            case 2:
                FinalConversation(other);
                break;
            default:
                break;

        }
    }

    public bool ExecuteOnRelease() { return false; }

    private void InitialConversation(GameObject other) {
        DialogueBox.instance.StartConversation(initialConversations[index % initialConversations.Count]);
        // index++;
        // if (index == initialConversations.Count) {
        //     index = 0;
        //     conversationIndex++;
        //     PlayerManager.instance.questStarted = true;
        //     questDoor.isLocked = false;
        // }
        // FocusOn focusOn = other.gameObject.GetComponent<FocusOn>();
        // focusOn.Focus(transform.position);
    }

    private void QuestConversation(GameObject other) {
        // if (PlayerManager.instance.hasFinalEye) {
        //     index = 0;
        //     conversationIndex++;
        //     FinalConversation(other);
        // } else {
        //     DialogueBox.instance.StartConversation(questConversations[index % questConversations.Count]);
        //     FocusOn focusOn = other.gameObject.GetComponent<FocusOn>();
        //     focusOn.Focus(transform.position);
        // }
    }

    private void FinalConversation(GameObject other) {
        // if (PlayerManager.instance.questComplete) {
        //     PlayerManager.instance.questComplete = false;
        //     endGame.gameObject.GetComponent<Collider>().enabled = true;
        //     index++;
        // }
        // DialogueBox.instance.StartConversation(finalConversations[index % finalConversations.Count]);
        // FocusOn focusOn = other.gameObject.GetComponent<FocusOn>();
        // focusOn.Focus(transform.position);
    }

    void OnTriggerEnter(Collider other) {
        // if (other.gameObject.tag == "FinalEye") {
        //     Destroy(other.transform.parent.gameObject);       
        //     PlayerManager.instance.questStarted = false;
        //     PlayerManager.instance.questComplete = true;
        // }
    }

    public string[] Lines() {
        return new string[] { "",};
    }
}
