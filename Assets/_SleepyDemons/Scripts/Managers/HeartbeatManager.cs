using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class HeartbeatManager : MonoBehaviour
{
    [SerializeField] private AnxietyManager anxietyManager;
    [SerializeField] private AudioSource heartBeatAudioSource;
    [SerializeField] private float minBpm = 0f;
    [SerializeField] private float restingHeartRate = 60f;
    [SerializeField, Range(0, 210)] private float currentBpm = 60f;

    [SerializeField, ReadOnly] private float currentPitch;
    [SerializeField] private float anxietyScalar;
    [SerializeField, Range(0, 210)] private float minVolumeBpm;
    [SerializeField, Range(0, 210)] private float maxVolumeBpm;

    private void Awake()
    {
        CalculatePitch();
    }

    void Update()
    {
        CalculatePitch();
        CalculateVolume();
    }

    void CalculatePitch()
    {
        currentBpm = restingHeartRate + restingHeartRate * (anxietyManager.OverallAnxiety * anxietyScalar);
        currentPitch = (currentBpm - minBpm) / (restingHeartRate - minBpm);
        heartBeatAudioSource.pitch = currentPitch;
    }

    void CalculateVolume()
    {
        heartBeatAudioSource.volume = (currentBpm - minVolumeBpm) / (maxVolumeBpm - minVolumeBpm);
    }
}
