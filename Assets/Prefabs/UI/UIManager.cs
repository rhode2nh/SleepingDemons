using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public IUIPanel _pauseUI;
    public IUIPanel _inventoryUI;
    public IUIPanel _marketUI;

    [SerializeField] private GameObject _pause;
    [SerializeField] private GameObject _inventory;
    [SerializeField] private GameObject _market;


    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        _pauseUI = _pause.GetComponent<IUIPanel>();
        _inventoryUI = _inventory.GetComponent<IUIPanel>();
        _marketUI = _market.GetComponent<IUIPanel>();
    }

    public void OpenPanel(IUIPanel panel)
    {
        panel.OpenPanel();
    }

    public void ClosePanel(IUIPanel panel)
    {
        // temp fix until root issue can be figured out.
        PlayerManager.instance.isHolding = false;
        panel.ClosePanel();
    }
}
