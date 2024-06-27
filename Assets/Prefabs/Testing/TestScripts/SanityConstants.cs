using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanityConstants : MonoBehaviour
{
    public static SanityConstants Instance;
    
    [field: Header("Short Anxiety Properties")]
    [field: SerializeField, Range(0, 1)] public float AnxietyShortDesiredAmount { get; private set; }
    [field: SerializeField] public float AnxietyShortDuration { get; private set; }
    [field: SerializeField, Range(0, 1)] public float AnxietyShortAttackRate { get; private set; }
    [field: SerializeField, Range(0, 10)] public float AnxietyShortDesiredRate { get; private set; }
    [field: SerializeField, Range(0, 10)] public float AnxietyShortInitialRate { get; private set; }

    [field: Header("Medium Anxiety Properties")]
    [field: SerializeField, Range(0, 1)] public float AnxietyMediumDesiredAmount { get; private set; }
    [field: SerializeField] public float AnxietyMediumDuration { get; private set; }
    [field: SerializeField, Range(0, 1)] public float AnxietyMediumAttackRate { get; private set; }
    [field: SerializeField, Range(0, 10)] public float AnxietyMediumDesiredRate { get; private set; }
    [field: SerializeField, Range(0, 10)] public float AnxietyMediumInitialRate { get; private set; }
    
    [field: Header("Long Anxiety Properties")]
    [field: SerializeField, Range(0, 1)] public float AnxietyLongDesiredAmount { get; private set; }
    [field: SerializeField] public float AnxietyLongDuration { get; private set; }
    [field: SerializeField, Range(0, 1)] public float AnxietyLongAttackRate { get; private set; }
    [field: SerializeField, Range(0, 10)] public float AnxietyLongDesiredRate { get; private set; }
    [field: SerializeField, Range(0, 10)] public float AnxietyLongInitialRate { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}
