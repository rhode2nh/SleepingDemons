using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using StarterAssets;
using UnityEngine;
using Random = UnityEngine.Random;

public class Zone : MonoBehaviour
{
    private Observer _observer;
    private List<IObservable> _observables = new();
    private FirstPersonController _player;
    private Coroutine _rollCoroutine;
    private void Start()
    {
        _observer = Observer.Instance;
    }
    private void OnTriggerEnter(Collider other)
    {
        _player = other.GetComponent<FirstPersonController>();
        if (_player != null)
        {
            _rollCoroutine = StartCoroutine(TryForRoll());
        }

        IObservable observable = other.GetComponent<IObservable>();
        if (observable != null)
        {
            _observables.Add(observable);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<FirstPersonController>() != null)
        {
            _player = null;
            StopCoroutine(_rollCoroutine);
        }
    }

    private IEnumerator TryForRoll()
    {
        int attempts = 0;
        bool roll = false;
        while (attempts < 3)
        {
            roll = _observer.Roll();
            attempts++;
            if (!roll)
            {
                yield return new WaitForSeconds(1.0f);
            }
        }
        
        float totalWeight = _observables.Sum(x => x.RollChance);
        float randomValue = Random.Range(0.0f, totalWeight);

        float currentWeight = 0.0f;
        foreach (var observable in _observables)
        {
            currentWeight += observable.RollChance;
            if (!(randomValue <= currentWeight)) continue;

            StartCoroutine(WaitForConditions(observable));
            break;
        }
    }

    private IEnumerator WaitForConditions(IObservable observable)
    {
        Debug.Log("Waiting for conditions");
        // yield return new WaitUntil(() => observable.IsLookingAt(Camera.main.gameObject) && observable.IsWithinRange(_player.gameObject));
        bool isLookingAt = false;
        bool isWithinRange = false;
        while (!isLookingAt || !isWithinRange)
        {
            if (_player == null) yield break;

            isLookingAt = observable.IsLookingAt(Camera.main.gameObject);
            isWithinRange = observable.IsWithinRange(_player.gameObject);
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("Success");
        observable.OnRollSuccess();
    }
}
