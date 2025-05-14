using System;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    [SerializeField] public float _checkChamberTimerDuration;
    [SerializeField] [ReadOnly] private List<string> _lastActionMap;
    private PlayerInput _playerInput;
    private StarterAssetsInputs _input;

    public bool IsFlashlightOn;
    public Vector2 MouseVelocity { get; private set; }
    public float LookVelocity { get; private set; }

    public Action<bool> OnInteract;

    private void Awake()
    {
        Instance = this;
        _playerInput = GetComponent<PlayerInput>();
        _input = GetComponent<StarterAssetsInputs>();
    }

    private void Start()
    {
        _lastActionMap = new List<string>
        {
            _playerInput.currentActionMap.name
        };
    }

    private void Update()
    {
        LookVelocity = _input.Look.magnitude;
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

    public void SwitchCurrentActionMap(string actionMap)
    {
        _playerInput.SwitchCurrentActionMap(actionMap);
    }
}
