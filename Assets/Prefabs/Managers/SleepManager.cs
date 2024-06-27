using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SleepManager : MonoBehaviour
{
    // The amount of energy remaining until the player should go to sleep.
    [SerializeField] private int _energyRemaining;
    [SerializeField, ReadOnly] private string formattedEnergyRemaining;
    [SerializeField] private int _previousSleepAmount;
    [SerializeField] private int _timeSinceLastRest;
    [SerializeField, ReadOnly] private string formattedTimeSinceLastRest;
    [SerializeField] private int _sleepThreshold;
    [SerializeField, ReadOnly] private int OPTIMAL_SLEEP = 28800;
    [SerializeField, ReadOnly] private int _amountToSleep;

    private void Start()
    {
        TimeManager.Instance.onProcessTick += ProcessTick;
        _amountToSleep = OPTIMAL_SLEEP;
    }

    private void ProcessTick(int tick)
    {
        _energyRemaining -= tick;
        _timeSinceLastRest += tick;
        formattedEnergyRemaining = TimeManager.Instance.FormattedTime(_energyRemaining);
        formattedTimeSinceLastRest = TimeManager.Instance.FormattedTime(_timeSinceLastRest);
    }

    public void Sleep()
    {
        if (_timeSinceLastRest <= _sleepThreshold) return;
        
        CalculateAmountToSleep();
        TimeManager.Instance.ProgressTime(_amountToSleep);
        CalculateEnergyRemaining();
        _previousSleepAmount = _amountToSleep;
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
        _energyRemaining -= OPTIMAL_SLEEP - _amountToSleep;
    }
}
