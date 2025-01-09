using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairManager : MonoBehaviour
{
    public static CrosshairManager instance;
    public GameObject interactIcon;
    void Awake() {
        instance = this;
    }

    void Start() {
        interactIcon.SetActive(false);
    }
    
    public void ShowCrossHair() {
        gameObject.SetActive(true);
    }

    public void HideCrossHair() {
        gameObject.SetActive(false);
    }

    public void ShowInteractIcon() {
        interactIcon.SetActive(true);
    }

    public void HideInteractIcon() {
        interactIcon.SetActive(false);
    }
}
