using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class FlickeringLights : MonoBehaviour
{
    [Header("Flicker Values")]
    [SerializeField, ReadOnly] private float timeSinceLastFlicker;
    
    [Header("Flicker Settings")]
    [SerializeField] private Light flickeringLight;
    [SerializeField] private float onIntervalSeconds;
    [SerializeField] private float offIntervalSeconds;
    
    [Header("Flicker Randomness")]
    [SerializeField, Range(0, 1)] private float randomness;
    [SerializeField, Range(0, 100)] private float offMinRandomRange;
    [SerializeField, Range(0, 100)] private float offMaxRandomRange;
    [SerializeField, Range(0, 100)] private float onMinRandomRange;
    [SerializeField, Range(0, 100)] private float onMaxRandomRange;

    private float _calculatedOffInterval;
    private float _calculatedOnInterval;

    IEnumerator Start()
    {
        while (true)
        {
            if (!flickeringLight.enabled)
            {
                _calculatedOffInterval = CalculateFlickerValues(offIntervalSeconds, offMinRandomRange, offMaxRandomRange, randomness);
                yield return new WaitForSeconds(_calculatedOffInterval);
            }
            else
            {
                _calculatedOnInterval = CalculateFlickerValues(onIntervalSeconds, onMinRandomRange, onMaxRandomRange, randomness);
                yield return new WaitForSeconds(_calculatedOnInterval);
            }

            flickeringLight.enabled = !flickeringLight.enabled;
        }
    }

    private float CalculateFlickerValues(float interval, float minRange, float maxRange, float randomness)
    {
        var randomInterval = Random.Range(minRange, maxRange) * randomness;

        return interval + randomInterval;
    }
}
