using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EyeCount : MonoBehaviour
{
    private TMP_Text _text;

    void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _text.SetText("Eyes: " + InventoryManager.instance.eyes.ToString());
        InventoryManager.instance.OnUpdateEyeGUI += UpdateUI;
    }

    // Update is called once per frame
    void UpdateUI(int eyeCount)
    {
        _text.SetText("Eyes: " + eyeCount.ToString());
    }
}
