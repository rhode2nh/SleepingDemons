using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LightController : MonoBehaviour
{
    [SerializeField] private LightSwitch _lightSwitch;
    private LightBulb _lightBulb;
    
    [Header("Flicker Settings")]
    [SerializeField] private bool _enableFlicker;

    [SerializeField] private float _flickerFrequency;
    [SerializeField, Range(0, 1)] private float _flickerRandomness;
    [SerializeField] private float _minRandom;
    [SerializeField] private float _maxRandom;
    
    public event Action<float> OnDimLightBulb;

    private void Awake()
    {
        _lightBulb = GetComponentInChildren<LightBulb>();
    }

    protected virtual void OnEnable()
    {
        _lightSwitch.OnDimLight += DimLight;
    }
    
    protected virtual void OnDisable()
    {
        _lightSwitch.OnDimLight -= DimLight;
    }

    public void RemoveLightBulb(LightBulb lightBulb)
    {
        lightBulb.gameObject.SetActive(false);
        StopCoroutine(Flicker());
        lightBulb.Spawn();
    }

    private void ReplaceLightBulb(LightBulbData lightBulbData)
    {
        _enableFlicker = lightBulbData.Flicker;
        _lightBulb.Replace(lightBulbData);
        if (_lightSwitch.IsOn)
        {
            DimLight(1.0f);
        }
    }

    protected virtual IEnumerator Flicker()
    {
        var currentIntensity = _lightSwitch.InitialIntensity;
        while (_lightSwitch.IsOn && _enableFlicker)
        {
            FlickerLights(currentIntensity);
            currentIntensity = currentIntensity > 0.0f ? 0.0f : _lightSwitch.InitialIntensity;
            yield return new WaitForSeconds(_flickerFrequency + _flickerFrequency * Random.Range(-_flickerRandomness + _minRandom, _flickerRandomness + _maxRandom));
        }
        yield return null;
    }

    private void FlickerLights(float intensity)
    {
        OnDimLightBulb?.Invoke(intensity);
    }

    protected virtual void DimLight(float intensity)
    {
        if (_lightSwitch.IsOn && _enableFlicker)
        {
            StartCoroutine(Flicker());
        }
        OnDimLightBulb?.Invoke(intensity);
    }

    private void OnDrawGizmos()
    {
        if (_lightSwitch == null) return;
        
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, _lightSwitch.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        var lightBulbData = other.GetComponent<LightBulbData>();
        if (lightBulbData == null) return;
        if (lightBulbData.JustSpawned) return;
        
        ReplaceLightBulb(lightBulbData);
    }

    private void OnTriggerExit(Collider other)
    {
        var lightBulbData = other.GetComponent<LightBulbData>();
        if (lightBulbData == null) return;
        if (lightBulbData.JustSpawned) lightBulbData.JustSpawned = false;
    }
}
