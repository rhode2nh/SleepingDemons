using UnityEngine;

public class Lamp : LightController
{
    [SerializeField] private Renderer lampShadeRenderer;
    [SerializeField] private float emissiveIntensity = 1;
    [SerializeField] private AudioSource lampSwitchOn;
    [SerializeField] private AudioSource lampSwitchOff;

    private Color _emissiveColor;

    void Awake()
    {
        _emissiveColor = lampShadeRenderer.material.GetColor("_EmissionColor");
    }
    
    public override void SwitchLight()
    {
        base.SwitchLight();
        if (light.enabled)
        {
            ChangeLampState(lampSwitchOn, emissiveIntensity);
        }
        else
        {
            ChangeLampState(lampSwitchOff, 0);
        }
    }

    private void ChangeLampState(AudioSource switchSfx, float intensity)
    {
        switchSfx.Play();
        lampShadeRenderer.material.SetColor("_EmissionColor", _emissiveColor * intensity);
    }

    public override void ExecutePostTrigger()
    {
        if (light.enabled)
        {
            ChangeLampState(lampSwitchOn, emissiveIntensity);
        }
        else
        {
            ChangeLampState(lampSwitchOff, 0);
        }
    }
}
