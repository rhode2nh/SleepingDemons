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
    [field: Header("Calculated Values")]
    [field: SerializeField, ReadOnly] public float OverallAnxiety { get; private set; }
    [field: SerializeField, ReadOnly] public float BaseAnxiety { get; private set; }
    [field: SerializeField, ReadOnly] public float EventAnxiety { get; private set; }
    [field: SerializeField, ReadOnly] public float ProximalAnxiety { get; private set; }
    
    [field: Header("Weights")]
    [field: SerializeField, Range(0.01f, 1)] public float BaseAnxietyWeight { get; private set; }
    [field: SerializeField, Range(0.01f, 1)] public float ProximalAnxietyWeight { get; private set; }
    [field: SerializeField, Range(0.01f, 1)] public float EventAnxietyWeight { get; private set; }
    
    [field: Header("Event Anxiety Testing")]
    [SerializeField] private bool addShortAnxietyEvent;
    [SerializeField] private bool addMediumAnxietyEvent;
    
    [field: Header("Base Anxiety")]
    [field: SerializeField, ReadOnly] public float EventAnxietyPenalty { get; private set; }
    [field: SerializeField] public float EventAnxietyPenaltyScale { get; private set; }
    [field: SerializeField, ReadOnly] public float ProximalAnxietyPenalty { get; private set; }
    [field: SerializeField] public float ProximalAnxietyPenaltyScale { get; private set; }
    [field: SerializeField, ReadOnly] public float RaycastAnxietyPenalty { get; private set; }
    [field: SerializeField] public float RaycastAnxietyPenaltyScale { get; private set; }
    
    [field: Header("Event Anxiety")]
    [SerializeField] private List<AnxietyEvent> activeAnxietyEvents;
    [field: SerializeField] public AnxietyEvent GreatestAnxietyEvent { get; private set; }
    [SerializeField] private List<AnxietyEvent> anxietyEventHistory;
    [field: SerializeField] public int AnxietyEventCooldown { get; private set; }
    [field: SerializeField] public int AnxietyEventCooldownTimer { get; private set; }
    [field: SerializeField, ReadOnly] public bool IsCooldown { get; private set; }
    [field: SerializeField, ReadOnly] public int NumAnxietyEvents { get; private set; }
    [field: SerializeField] public int AnxietyEventCooldownThreshold { get; private set; }
    [field: SerializeField, Tooltip("Determines which events are considered for cooldown")]
    public int AnxietyHistoryTimeCheck { get; private set; }
    [field: SerializeField, Tooltip("How long to keep track of previous anxiety events")]
    public int AnxietyTimeHistoryLimit { get; private set; }
    
    [field: Header("Proximal Anxiety")]
    [field: SerializeField] public int MaxProximalAnxietySources { get; private set; }
    [field: SerializeField] public float ProximalDecay { get; private set; }

    [SerializeField] private List<ProximalAnxiety> proximalAnxietySources;
    void Awake()
    {
        activeAnxietyEvents = new List<AnxietyEvent>();
        anxietyEventHistory = new List<AnxietyEvent>();
        proximalAnxietySources = new List<ProximalAnxiety>();
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
        ProcessBaseAnxiety();
        ProcessProximalAnxiety();
        ProcessAnxietyEventHistory();
        ProcessRaycastAnxiety();
        
        CheckCooldown(tick);
        
        // Calculate overall anxiety after everything else
        ProcessOverallAnxiety();
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

    void ProcessBaseAnxiety()
    {
        BaseAnxiety = EventAnxietyPenalty + ProximalAnxietyPenalty;
    }

    void ProcessRaycastAnxiety()
    {
        
    }

    void ProcessProximalAnxiety()
    {
        if (proximalAnxietySources.Count > 0)
        {
            var sumOfSources = proximalAnxietySources.Sum(x => x.GetCurrentProximalRate());
            ProximalAnxiety = Mathf.Clamp(ProximalAnxiety + sumOfSources * Time.deltaTime, 0, 1);
            
            var sumOfPenalties = proximalAnxietySources.Sum(x => x.CurrentPenalty) * ProximalAnxietyPenaltyScale;
            ProximalAnxietyPenalty = Mathf.Clamp(sumOfPenalties, 0, 1);
        }
        else
        {
            ProximalAnxiety = Mathf.Clamp( ProximalAnxiety - ProximalDecay * Time.deltaTime, 0, 1);
            ProximalAnxietyPenalty = 0.0f;
        }
    }

    public void AddProximalSource(ProximalAnxiety source)
    {
        if (proximalAnxietySources.Count < MaxProximalAnxietySources)
        {
            proximalAnxietySources.Add(source);
        }
    }
    
    public void RemoveProximalSource(ProximalAnxiety source)
    {
        if (proximalAnxietySources.Count < MaxProximalAnxietySources)
        {
            proximalAnxietySources.Remove(source);
        }
    }

    void ProcessOverallAnxiety()
    {
        var weightSum = BaseAnxietyWeight + ProximalAnxietyWeight + EventAnxietyWeight;
        var normalizedBWeight = BaseAnxietyWeight / weightSum;
        var normalizedPWeight = ProximalAnxietyWeight / weightSum;
        var normalizedEWeight = EventAnxietyWeight / weightSum;

        OverallAnxiety = BaseAnxiety * normalizedBWeight + ProximalAnxiety * normalizedPWeight +
               EventAnxiety * normalizedEWeight;
    }

    void ProcessAnxietyEventHistory()
    {
        anxietyEventHistory.RemoveAll(x => x.timestamp < TimeManager.Instance.GetTotalTime() - AnxietyTimeHistoryLimit);
        NumAnxietyEvents = anxietyEventHistory.Count(x => x.timestamp >= TimeManager.Instance.GetTotalTime() - AnxietyHistoryTimeCheck);

        EventAnxietyPenalty = anxietyEventHistory.Count * EventAnxietyPenaltyScale;
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
            StartCoroutine(ProcessAnxietyEvent(anxietyEvent, OnAnxietyEventFinished));
        }
    }

    private bool TryProcessAnxietyEvent(AnxietyEvent anxietyEvent)
    {
        return !IsCooldown;
    }

    IEnumerator ProcessAnxietyEvent(AnxietyEvent anxietyEvent, Action<string> onAnxietyEventFinished)
    {
        activeAnxietyEvents.Add(anxietyEvent);
        anxietyEventHistory.Add(anxietyEvent);
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
                    EventAnxiety += anxietyEvent.initialRate * Time.deltaTime;
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
            EventAnxiety = Mathf.Clamp(EventAnxiety - rateToAdd, 0, 1);
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
