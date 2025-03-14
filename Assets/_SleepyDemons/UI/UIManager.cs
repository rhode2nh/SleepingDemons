using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public IUIPanel _pauseUI;
    public IUIPanel _inventoryUI;

    [SerializeField] private GameObject _pause;
    [SerializeField] private GameObject _inventory;


    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        _pauseUI = _pause.GetComponent<IUIPanel>();
        _inventoryUI = _inventory.GetComponent<IUIPanel>();
        _pauseUI.ClosePanel();
        _inventoryUI.ClosePanel();
    }

    public void OpenPanel(IUIPanel panel)
    {
        panel.OpenPanel();
    }

    public void ClosePanel(IUIPanel panel)
    {
        panel.ClosePanel();
    }
}
