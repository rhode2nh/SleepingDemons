using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public event Action OnOpenPauseUI;
    public event Action OnClosePauseUI;
    
    public event Action OnOpenInventoryUI;
    public event Action OnCloseInventoryUI;

    void Awake()
    {
        Instance = this;
    }

    public virtual void OpenPauseUI()
    {
        OnOpenPauseUI?.Invoke();
    }

    public virtual void ClosePauseUI()
    {
        OnClosePauseUI?.Invoke();
    }

    public virtual void OpenInventoryUI()
    {
        OnOpenInventoryUI?.Invoke();
    }

    public virtual void CloseInventoryUI()
    {
        OnCloseInventoryUI?.Invoke();
    }
}
