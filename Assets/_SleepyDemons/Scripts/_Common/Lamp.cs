using UnityEngine;

public class Lamp : LightController
{
    [SerializeField] private Renderer lampShadeRenderer;
    [SerializeField] private AudioSource lampSwitchOn;
    [SerializeField] private AudioSource lampSwitchOff;
    [SerializeField] private ChainGenerator chainGenerator;

    private Color _emissiveColor;

    void Awake()
    {
        _emissiveColor = lampShadeRenderer.material.GetColor("_EmissionColor");
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        chainGenerator.OnPullClick += PullClickSfx;
        chainGenerator.OnLetGo += LetGoSfx;
    }
    
    protected override void OnDisable()
    {
        base.OnDisable();
        chainGenerator.OnPullClick -= PullClickSfx;
        chainGenerator.OnLetGo -= LetGoSfx;
    }

    protected override void DimLight(float intensity)
    {
        base.DimLight(intensity);
        if (intensity > 0.0f)
        {
            ChangeLampState(1);
        }
        else
        {
            ChangeLampState(0);
        }
    }

    private void PullClickSfx()
    {
        lampSwitchOn.Play();
    }
    
    private void LetGoSfx()
    {
        lampSwitchOff.Play();
    }

    private void ChangeLampState(float intensity)
    {
        lampShadeRenderer.material.SetColor("_EmissionColor", _emissiveColor * intensity);
    }
}
