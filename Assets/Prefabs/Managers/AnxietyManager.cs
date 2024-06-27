using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public enum AnxietyType
{
    Short,
    Medium,
    Long
}

[Serializable]
public class AnxietyEvent
{
    public AnxietyEvent(AnxietyType anxietyType, int timestamp)
    {
        this.guid = Guid.NewGuid().ToString();
        this.timestamp = timestamp;
        switch (anxietyType)
        {
            case AnxietyType.Short:
                desiredAttack = SanityConstants.Instance.AnxietyShortDesiredAmount;
                duration = SanityConstants.Instance.AnxietyShortDuration;
                initialRate = SanityConstants.Instance.AnxietyShortInitialRate;
                desiredRate = SanityConstants.Instance.AnxietyShortDesiredRate;
                break;
            case AnxietyType.Medium:
                desiredAttack = SanityConstants.Instance.AnxietyMediumDesiredAmount;
                duration = SanityConstants.Instance.AnxietyMediumDuration;
                initialRate = SanityConstants.Instance.AnxietyMediumInitialRate;
                desiredRate = SanityConstants.Instance.AnxietyMediumDesiredRate;
                break;
            case AnxietyType.Long:
                desiredAttack = SanityConstants.Instance.AnxietyLongDesiredAmount;
                duration = SanityConstants.Instance.AnxietyLongDuration;
                initialRate = SanityConstants.Instance.AnxietyLongInitialRate;
                desiredRate = SanityConstants.Instance.AnxietyLongDesiredRate;
                break;
        }
        // this.weight = weight;
    }

    [SerializeField] public string guid;
    [SerializeField] public float currentAmount;
    [SerializeField] public float desiredAttack;
    [SerializeField] public float sustainDuration;
    [SerializeField] public float duration;
    [SerializeField] public float weight;
    
    [SerializeField] public float initialRate;
    [SerializeField] public float currentRate;
    [SerializeField] public float desiredRate;

    [SerializeField] public int timestamp;
    [SerializeField] public float initialStartValue;
    
    [SerializeField] public float sustainTimeElapsed;
    
    [SerializeField] public bool isAttacking;
    [SerializeField] public bool isSustaining;
    [SerializeField] public bool isDecaying;
    [SerializeField] public bool startValueFound;
    
    // TODO: Implement after attack, sustain, and decay have been implemented
    // Rate of change isn't necessarily important. Need to think more about it.
    // Calculates the rate of change 
    // private void CalculateRate()
    // {
    //     if (timeElapsed < duration)
    //     {
    //         currentRate += 
    //     }
    // }

    public float NormalizedWeight(float sum)
    {
        return weight / sum;
    }
}

public class AnxietyManager : MonoBehaviour
{
    [SerializeField] private List<AnxietyEvent> activeAnxietyEvents;
    [SerializeField] private List<AnxietyEvent> anxietyEventHistory;
    [SerializeField] private bool addShortAnxietyEvent;
    [SerializeField] private bool addMediumAnxietyEvent;
    [field: SerializeField] public int AnxietyEventCooldown { get; private set; }
    [field: SerializeField] public int AnxietyEventCooldownTimer { get; private set; }
    [field: SerializeField] public bool IsCooldown { get; private set; }
    [field: SerializeField] public int AnxietyEventCooldownThreshold { get; private set; }
    [field: SerializeField] public int AnxietyHistoryTimeCheck { get; private set; }
    [field: SerializeField] public int NumAnxietyEvents { get; private set; }
    [field: SerializeField] public float Current { get; private set;  }
    [field: SerializeField] public int AnxietyTimeHistoryLimit { get; private set; }
    [field: SerializeField] public AnxietyEvent GreatestAnxietyEvent { get; private set; }
    void Awake()
    {
        activeAnxietyEvents = new List<AnxietyEvent>();
        anxietyEventHistory = new List<AnxietyEvent>();
    }

    IEnumerator Start()
    {
        TimeManager.Instance.onProcessTick += ProcessTick;
        while (true)
        {
            GreatestAnxietyEvent = activeAnxietyEvents.OrderByDescending(x => x.currentAmount).FirstOrDefault();
            yield return new WaitForEndOfFrame();
        }
    }

    void ProcessTick(int tick)
    {
        ProcessAnxietyEventHistory();
        CheckCooldown(tick);
    }
    
    void Update()
    {
        if (addShortAnxietyEvent)
        {
            TestAnxietyEvent(ref addShortAnxietyEvent, AnxietyType.Short);
        }
        if (addMediumAnxietyEvent)
        {
            TestAnxietyEvent(ref addMediumAnxietyEvent, AnxietyType.Medium);
        }
    }

    void ProcessAnxietyEventHistory()
    {
        anxietyEventHistory.RemoveAll(x => x.timestamp < TimeManager.Instance.GetTotalTime() - AnxietyTimeHistoryLimit);
        NumAnxietyEvents = anxietyEventHistory.Count(x => x.timestamp >= TimeManager.Instance.GetTotalTime() - AnxietyHistoryTimeCheck);
    }

    void CheckCooldown(int tick)
    {
        if (NumAnxietyEvents >= AnxietyEventCooldownThreshold && !IsCooldown)
        {
            AnxietyEventCooldownTimer = AnxietyEventCooldown;
            IsCooldown = true;
        }

        if (IsCooldown)
        {
            AnxietyEventCooldownTimer -= tick;
            if (AnxietyEventCooldownTimer <= 0)
            {
                AnxietyEventCooldownTimer = 0;
                IsCooldown = false;
            }
        }
    }

    void TestAnxietyEvent(ref bool check, AnxietyType anxietyType)
    {
        check = false;
        AnxietyEvent anxietyEvent = new AnxietyEvent(anxietyType, TimeManager.Instance.GetTotalTime());
        if (TryProcessAnxietyEvent(anxietyEvent))
        {
            activeAnxietyEvents.Add(anxietyEvent);
            anxietyEventHistory.Add(anxietyEvent);
            StartCoroutine(ProcessAnxietyEvent(activeAnxietyEvents[^1], OnAnxietyEventFinished));
        }
    }

    private bool TryProcessAnxietyEvent(AnxietyEvent anxietyEvent)
    {
        return !IsCooldown;
    }

    IEnumerator ProcessAnxietyEvent(AnxietyEvent anxietyEvent, Action<string> onAnxietyEventFinished)
    {
        anxietyEvent.isAttacking = true;
        while (true)
        {
            if (anxietyEvent.isAttacking) Attack(anxietyEvent);
            if (anxietyEvent.isSustaining) Sustain(anxietyEvent);
            if (anxietyEvent.isDecaying) Decay(anxietyEvent);

            if (!anxietyEvent.isAttacking && !anxietyEvent.isSustaining && !anxietyEvent.isDecaying) break;
            yield return new WaitForEndOfFrame();
        }

        onAnxietyEventFinished.Invoke(anxietyEvent.guid);
        yield return null;
    }
    private void Attack(AnxietyEvent anxietyEvent)
    {
        if (anxietyEvent.currentAmount < anxietyEvent.desiredAttack)
        {
            anxietyEvent.currentAmount += anxietyEvent.initialRate * Time.deltaTime;
            
            // Only add to overall anxiety if it's the strongest event
            if (GreatestAnxietyEvent != null)
            {
                if (GreatestAnxietyEvent.guid.Equals(anxietyEvent.guid))
                {
                    Current += anxietyEvent.initialRate * Time.deltaTime;
                }
            }
        }
        else
        {
            anxietyEvent.isAttacking = false;
            anxietyEvent.isSustaining = true;
        }
    }

    private void Sustain(AnxietyEvent anxietyEvent)
    {
        if (anxietyEvent.sustainTimeElapsed <= anxietyEvent.sustainDuration)
        {
            anxietyEvent.sustainTimeElapsed += Time.deltaTime;
        }
        else
        {
            anxietyEvent.isSustaining = false;
            anxietyEvent.isDecaying = true;
        }
    }

    private void Decay(AnxietyEvent anxietyEvent)
    {
        // This tells the event to end early if there is another anxiety event with a greater value.
        if (!GreatestAnxietyEvent.guid.Equals(anxietyEvent.guid))
        {
            anxietyEvent.isDecaying = false;
            return;
        }
        
        if (anxietyEvent.currentAmount > 0.0f)
        {
            var rateToAdd = anxietyEvent.initialRate * Time.deltaTime;
            anxietyEvent.currentAmount -= rateToAdd;
            Current = Mathf.Clamp(Current - rateToAdd, 0, 1);
        }
        else
        {
            anxietyEvent.isDecaying = false;
        }
    }

    private void OnAnxietyEventFinished(string guid)
    {
        activeAnxietyEvents.RemoveAll(x => x.guid == guid);
    }
}
