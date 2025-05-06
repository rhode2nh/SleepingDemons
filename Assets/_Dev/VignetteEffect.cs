using System;
using UnityEngine;

[ExecuteInEditMode]
public class VignetteEffect : MonoBehaviour
{
    [SerializeField] private Material _vignetteMaterial;

    private void Awake()
    {
        ResetValues();
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (_vignetteMaterial != null)
        {
            Graphics.Blit(source, destination, _vignetteMaterial);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }

    public void SetIntensity(float intensity)
    {
        _vignetteMaterial.SetFloat("_Intensity", intensity);
    }

    public void SetSmoothness(float smoothness)
    {
        _vignetteMaterial.SetFloat("_Smoothness", smoothness);
    }

    public void ResetValues()
    {
        _vignetteMaterial.SetFloat("_Intensity", 1.0f);
        _vignetteMaterial.SetFloat("_Smoothness", 1.0f);
    }

}
