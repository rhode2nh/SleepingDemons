using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EndGame : MonoBehaviour, IInteractable
{
    public Image fadeToBlack;
    public GameObject text;
    public float initialFinalScreenDelay;
    public float finalScreenDelay;
    public float fadeDelta;
    public bool coroutineStarted = false;
    public void ExecuteInteraction(GameObject other) {
        fadeToBlack = GameObject.FindWithTag("End").GetComponent<Image>();
        StartCoroutine(FadeToBlack());
    }

    void Start() {
        text = GameObject.FindWithTag("EndText");
        text.SetActive(false);
    }

    void Update() {
        // if (PlayerManager.instance.endGame && !coroutineStarted) {
        //     coroutineStarted = true;
        //     StartCoroutine(FinalScreen());
        // }
    }

    public bool ExecuteOnRelease() { return false; }

    IEnumerator FadeToBlack() {
        Color c = fadeToBlack.color;
        for (float fadeAmount = 0.0f; fadeAmount < 1.0f; fadeAmount += fadeDelta) {
            c.a = fadeAmount;
            fadeToBlack.color = c;
            yield return new WaitForEndOfFrame();
        }
        DialogueBox.instance.StartConversation("...What a weird dream");
        FocusOn.instance.Focus(transform.position);
        while(FocusOn.instance.isFocusing) {
            yield return new WaitForEndOfFrame();
        }
        // PlayerManager.instance.endGame = true;
    }

    IEnumerator FinalScreen() {
        yield return new WaitForSeconds(initialFinalScreenDelay); 
        text.gameObject.SetActive(true);
        // text.text = "The End";
        yield return new WaitForSeconds(finalScreenDelay);
        // save any game data here
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
