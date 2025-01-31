using System;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    public event Action<float> OnDimLight;
    public float initialIntensity;
    public bool isOn;

    private float currentIntensity;

    private void Start()
    {
        currentIntensity = initialIntensity;
        InitializeLights();
    }

    private void InitializeLights()
    {
        OnDimLight?.Invoke(initialIntensity);
        if (isOn)
        {
            OnDimLight?.Invoke(currentIntensity);
        }
        else
        {
            OnDimLight?.Invoke(0);
        }
    }

    public void SwitchLight()
    {
        isOn = !isOn;
        if (isOn)
        {
            OnDimLight?.Invoke(currentIntensity);
        }
        else
        {
            OnDimLight?.Invoke(0);
        }
    }
    
    public void DimLight(float intensity)
    {
        currentIntensity = intensity;
        OnDimLight?.Invoke(currentIntensity);
    }
}
