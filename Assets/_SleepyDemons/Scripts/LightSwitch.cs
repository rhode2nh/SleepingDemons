using System;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    public event Action<float> OnDimLight;
    
    [field: SerializeField] public float InitialIntensity { get; private set; }
    [field: SerializeField] public bool IsOn { get; private set; }

    private float _currentIntensity;

    private void Start()
    {
        _currentIntensity = InitialIntensity;
        InitializeLights();
    }

    private void InitializeLights()
    {
        OnDimLight?.Invoke(InitialIntensity);
        if (IsOn)
        {
            OnDimLight?.Invoke(_currentIntensity);
        }
        else
        {
            OnDimLight?.Invoke(0);
        }
    }

    public void SwitchLight()
    {
        IsOn = !IsOn;
        if (IsOn)
        {
            OnDimLight?.Invoke(_currentIntensity);
        }
        else
        {
            OnDimLight?.Invoke(0);
        }
    }
    
    public void DimLight(float intensity)
    {
        _currentIntensity = intensity;
        OnDimLight?.Invoke(_currentIntensity);
    }
}
