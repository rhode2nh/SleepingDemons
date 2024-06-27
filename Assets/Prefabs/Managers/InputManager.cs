using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    [SerializeField] public float _checkChamberTimerDuration;
    [SerializeField] [ReadOnly] private List<string> _lastActionMap;
    [SerializeField] private PlayerInput _playerInput;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        _lastActionMap = new List<string>
        {
            _playerInput.currentActionMap.name
        };
    }

    public void OpenPanel(IUIPanel panel, string actionMap)
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        _lastActionMap.Add(_playerInput.currentActionMap.name);
        _playerInput.SwitchCurrentActionMap(actionMap);
        UIManager.instance.OpenPanel(panel);
    }

    public void ClosePanel(IUIPanel panel)
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        string mapToSwitchTo = _lastActionMap[_lastActionMap.Count - 1];
        _lastActionMap.RemoveAt(_lastActionMap.Count - 1);
        _playerInput.SwitchCurrentActionMap(mapToSwitchTo);
        UIManager.instance.ClosePanel(panel);
    }

    public void SwitchCurrentActionMap(string actionMap)
    {
        _playerInput.SwitchCurrentActionMap(actionMap);
    }
}
