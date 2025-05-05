using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class TeleportToMaintenanceCloset : MonoBehaviour
{
    private LightSwitch _lightSwitch;
    [SerializeField] private LightSwitch _otherLightSwitch;
    private FirstPersonController _player;
    private CharacterController _characterController;

    private void Awake()
    {
        _lightSwitch = GetComponentInChildren<LightSwitch>();
        _player = FindAnyObjectByType<FirstPersonController>();
        _characterController = _player.GetComponent<CharacterController>();

    }

    private void Start()
    {
        _lightSwitch.OnSwitchLight += TeleportPlayer;
    }

    private void TeleportPlayer()
    {
        Vector3 localPos = _lightSwitch.transform.InverseTransformPoint(_player.transform.position);
        Quaternion localRot = Quaternion.Inverse(_lightSwitch.transform.rotation) * _player.transform.rotation;
        _characterController.enabled = false;
        _player.transform.position = _otherLightSwitch.transform.TransformPoint(localPos);
        _player.transform.rotation = _otherLightSwitch.transform.rotation * localRot;
        _characterController.enabled = true;
        _otherLightSwitch.SwitchLight();
    }
}
