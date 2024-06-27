using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketCinematicController : MonoBehaviour
{
    [SerializeField] private MarketAnimationController _marketAnimationController;

    void Start()
    {
        StartCoroutine(_marketAnimationController.CloseMarket());
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(_marketAnimationController.OpenMarket());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(_marketAnimationController.CloseMarket());
        }
    }
}
