using UnityEngine;

public class LightSwitchHandle : Interactable 
{
    private LightSwitch _lightSwitch;

    void Start()
    {
        _lightSwitch = GetComponentInParent<LightSwitch>();
    }

    public override void ExecuteInteraction(GameObject other)
    {
        _lightSwitch.SwitchLight();
        transform.eulerAngles = new Vector3(-transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
    }
}
