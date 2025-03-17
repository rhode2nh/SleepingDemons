using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    [SerializeField] public float _checkChamberTimerDuration;
    [SerializeField] [ReadOnly] private List<string> _lastActionMap;
    private PlayerInput _playerInput;

    public bool IsFlashlightOn;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _playerInput = FindObjectOfType<PlayerInput>();
        _lastActionMap = new List<string>
        {
            _playerInput.currentActionMap.name
        };
    }

    public void OpenUI(string actionMap)
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        _lastActionMap.Add(_playerInput.currentActionMap.name);
        _playerInput.SwitchCurrentActionMap(actionMap);
    }
    
    public void CloseUI()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        string mapToSwitchTo = _lastActionMap[^1];
        _lastActionMap.RemoveAt(_lastActionMap.Count - 1);
        _playerInput.SwitchCurrentActionMap(mapToSwitchTo);
    }

    public void OpenPanel(IUIPanel panel, string actionMap)
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        _lastActionMap.Add(_playerInput.currentActionMap.name);
        _playerInput.SwitchCurrentActionMap(actionMap);
        UIManager.Instance.OpenPanel(panel);
    }

    public void ClosePanel(IUIPanel panel)
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        string mapToSwitchTo = _lastActionMap[^1];
        _lastActionMap.RemoveAt(_lastActionMap.Count - 1);
        _playerInput.SwitchCurrentActionMap(mapToSwitchTo);
        UIManager.Instance.ClosePanel(panel);
    }

    public void SwitchCurrentActionMap(string actionMap)
    {
        _playerInput.SwitchCurrentActionMap(actionMap);
    }
}
