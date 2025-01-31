using System;
using UnityEngine;

public class LightController : MonoBehaviour
{
    [SerializeField] private LightSwitch lightSwitch;

    public event Action<float> OnDimLightBulb;

    protected virtual void OnEnable()
    {
        lightSwitch.OnDimLight += DimLight;
    }
    
    protected virtual void OnDisable()
    {
        lightSwitch.OnDimLight -= DimLight;
    }

     protected virtual void DimLight(float intensity)
    {
        OnDimLightBulb?.Invoke(intensity);
    }

    private void OnDrawGizmos()
    {
        if (lightSwitch == null) return;
        
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, lightSwitch.transform.position);
    }
}
