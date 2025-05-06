using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SleepManager : MonoBehaviour
{
    // The amount of energy remaining until the player should go to sleep.
    [Header("Sleep Info")]
    [SerializeField, ReadOnly] private string formattedEnergyRemaining;
    [SerializeField, ReadOnly] private int _previousSleepAmount;
    [SerializeField, ReadOnly] private string formattedTimeSinceLastRest;
    [SerializeField, ReadOnly] private int _sleepThreshold;
    [SerializeField, ReadOnly] private int potentialSleep;
    
    [field: Header("Penalties")]
    [field: SerializeField, ReadOnly] public float EnergyPenalty { get; private set; }
    [field: SerializeField] public float EnergyPenaltyRate { get; private set; }
    [field: SerializeField] public float MaxEnergyPenalty { get; private set; }
    [field: SerializeField] public int EnergyPenaltyInterval { get; private set; }
    [field: SerializeField, ReadOnly] public int EnergyPenaltyTime { get; private set; }
    
    private int _energyRemaining;
    private int _timeSinceLastRest;
    private int OPTIMAL_SLEEP = 28800;
    private int FULL_DAY = 86400;

    public static SleepManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        TimeManager.Instance.onProcessTick += ProcessTick;
        potentialSleep = OPTIMAL_SLEEP;
        _sleepThreshold = FULL_DAY - OPTIMAL_SLEEP;
        _energyRemaining = FULL_DAY - OPTIMAL_SLEEP;
    }

    private void ProcessTick(int tick)
    {
        _energyRemaining -= tick;
        _timeSinceLastRest += tick;
        formattedEnergyRemaining = TimeManager.Instance.FormattedTime(_energyRemaining);
        formattedTimeSinceLastRest = TimeManager.Instance.FormattedTime(_timeSinceLastRest);
        
        ProcessSleepPenalty(tick);
    }

    private void ProcessSleepPenalty(int tick)
    {
        if (_energyRemaining > 0) return;

        if (EnergyPenalty < MaxEnergyPenalty)
        {
            EnergyPenaltyTime += tick;
        }
        
        if (EnergyPenaltyTime > EnergyPenaltyInterval && EnergyPenalty < MaxEnergyPenalty)
        {
            EnergyPenalty += EnergyPenaltyRate;
            EnergyPenaltyTime = 0;
        }
    }

    public void Sleep()
    {
        if (_timeSinceLastRest <= _sleepThreshold) return;
        
        CalculateAmountToSleep();
        TimeManager.Instance.ProgressTime(potentialSleep);
        CalculateEnergyRemaining();
        _previousSleepAmount = potentialSleep;
        _timeSinceLastRest = 0;
        // SanityManager.Instance.AnxietyManager.DecreaseSleepPenalty(_previousSleepAmount);
    }

    private void CalculateAmountToSleep()
    {
        // _amountToSleep -= SanityManager.Instance.AnxietyManager.GetCalculatedSleepPenalty();
    }

    private void CalculateEnergyRemaining()
    {
        _energyRemaining = 57600;
        _energyRemaining -= OPTIMAL_SLEEP - potentialSleep;
    }
}
