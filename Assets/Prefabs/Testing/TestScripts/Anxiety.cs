using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using TMPro.Examples;
using UnityEngine;

public class Anxiety : MonoBehaviour
{
    [SerializeField] private AnxietyType anxietyType;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // other.gameObject.GetComponent<AnxietyManager>()?.StartAnxietyEvent(anxietyType);
        if (other.gameObject.GetComponent<FirstPersonController>() != null)
        {
            // SanityManager.Instance.AnxietyManager2.StartAnxietyEvent(anxietyType);
        }
    }
}
