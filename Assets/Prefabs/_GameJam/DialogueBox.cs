using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq.Expressions;

public class DialogueBox : MonoBehaviour
{
    public static DialogueBox instance;
    public TextMeshProUGUI textComponent;
    public List<string> lines;
    public float textSpeed;
    public float periodDelay;
    public GameObject panel;

    private int index;

    void Awake() {
        if (instance != null) {
            Debug.LogError("Found more than one Dialogue Box!");
        }
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;
        panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (panel.activeSelf) {
            if (Input.GetMouseButtonDown(0)) {
                if (textComponent.text == lines[index]) {
                    NextLine();
                } else {
                    StopAllCoroutines();
                    textComponent.text = lines[index];
                }
            }
        }
    }

    public void StartDialogue() {
        index = 0;
        StartCoroutine(TypeLine());
    }

    public void StartConversation(List<string> lines) {
        OpenDialogueBox();
        SetLines(lines);
        StartDialogue();
    }

    public void StartConversation(string line) {
        OpenDialogueBox();
        SetLines(new List<string> { line });
        StartDialogue();
    }

    IEnumerator TypeLine() {
        foreach (char c in lines[index].ToCharArray()) {
            textComponent.text += c;
            if (c == '.') {
                yield return new WaitForSeconds(periodDelay);
            } else {
                yield return new WaitForSeconds(textSpeed);
            }
        }
    }

    void NextLine() {
        if (index < lines.Count - 1) {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        } else {
            CloseDialogueBox();
        }
    }

    public void OpenDialogueBox() {
        panel.SetActive(true);
        CrosshairManager.instance.HideCrossHair();
    }

    public void CloseDialogueBox() {
        ClearLines();
        panel.SetActive(false);
        FocusOn.instance.UnFocus();
        CrosshairManager.instance.ShowCrossHair();
    }
    
    public void SetLines(List<string> lines) {
        this.lines = new List<string>(lines);
    }

    public void ClearLines() {
        textComponent.text = string.Empty;
        lines.Clear();
    }
}
