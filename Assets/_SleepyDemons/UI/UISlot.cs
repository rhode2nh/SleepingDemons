using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image _icon;
    [SerializeField] private Image _activeImage;
    [SerializeField] private Text title;
    private int _index;

    public Sprite Icon { set => _icon.sprite = value; }
    public int Index { get => _index; set => _index = value; }
    public Action<PointerEventData, int> OnPointerClickFunc { get; set; }

    void Awake()
    {
        SetActive(false);
    }

    public void SetIconActive(bool active)
    {
        _icon.gameObject.SetActive(active);
    }

    public void SetActive(bool active)
    {
        _activeImage.gameObject.SetActive(active);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnPointerClickFunc(eventData, Index);
    }

    public void SetSlot(Sprite icon)
    {
        Icon = icon;
        SetIconActive(true);
    }

    public void ClearSlot()
    {
        Icon = null;
        // Index = -1;
        SetIconActive(false);
    }
}

