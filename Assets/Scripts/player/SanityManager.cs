using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanityManager : MonoBehaviour
{
    public static SanityManager Instance;

    public float anxiety;
    public float sleep;
    public float paranoia;

    public AnxietyManager AnxietyManager;
    public SleepManager SleepManager;

    void Awake()
    {
        Instance = this;
    }
}
