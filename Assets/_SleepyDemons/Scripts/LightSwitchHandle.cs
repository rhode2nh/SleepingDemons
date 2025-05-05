using System;
using UnityEngine;

public class LightSwitchHandle : Interactable 
{
    private LightSwitch _lightSwitch;

    void Start()
    {
        _lightSwitch = GetComponentInParent<LightSwitch>();
        _lightSwitch.OnSwitchLight += SwitchHandle;
        if (_lightSwitch.IsOn)
        {
            SwitchHandle();
        }
    }

    private void SwitchHandle()
    {
        transform.eulerAngles = new Vector3(-transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
    }

    public override void ExecuteInteraction(GameObject other)
    {
        _lightSwitch.SwitchLight();
    }
}
