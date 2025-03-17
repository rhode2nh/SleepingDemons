using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairManager : MonoBehaviour
{
    public static CrosshairManager Instance;

    private Crosshair _crosshair;
    private InteractCrosshair _interactCrosshair;
    void Awake() {
        Instance = this;
        _crosshair = GetComponentInChildren<Crosshair>();
        _interactCrosshair = GetComponentInChildren<InteractCrosshair>();
    }
    
    public void ShowCrossHair() {
        _crosshair.gameObject.SetActive(true);
    }

    public void HideCrossHair() {
        _crosshair.gameObject.SetActive(false);
    }

    public void ShowInteractIcon(Sprite icon)
    {
        _interactCrosshair.Open(icon);
    }

    public void HideInteractIcon() {
        _interactCrosshair.Close();
    }
}
