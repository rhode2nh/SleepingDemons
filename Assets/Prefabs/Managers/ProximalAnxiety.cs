using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximalAnxiety : MonoBehaviour, IProximalAnxiety
{
    [field: SerializeField, Range(0, 1)] public float ProximalAnxietyRate { get; private set; }
    [field: SerializeField, Range(0, 2)] public float DistanceScale { get; private set; }
    [field: SerializeField] public float ProximalSourceOffset { get; private set; }
    [field: SerializeField] public float ProximalAnxietyPenaltyRate { get; private set; }
    [field: SerializeField] public float PenaltyRateTimeThreshold { get; private set; }
    [field: SerializeField] public float MaxPenaltyRateTime { get; private set; }
    [field: SerializeField] public float PenaltyTimeInterval { get; private set; }
    [field: SerializeField] public float CurrentPenalty { get; private set; }
    
    [SerializeField] private SphereCollider _proximalArea;
    
    private GameObject _player;
    
    public float GetCurrentProximalRate()
    {
        var dirTowardsPlayer = (_player.transform.position - gameObject.transform.position).normalized;
        var distance = Vector3.Distance(gameObject.transform.position + dirTowardsPlayer * ProximalSourceOffset, _player.transform.position);

        var startPoint = 0.0f;
        var startToPlayer = Vector3.Distance(gameObject.transform.position + dirTowardsPlayer * ProximalSourceOffset, _player.transform.position);
        var startToEnd = Vector3.Distance(gameObject.transform.position + dirTowardsPlayer * ProximalSourceOffset, gameObject.transform.position + dirTowardsPlayer * _proximalArea.radius);
        
        return ProximalAnxietyRate + (1 - ((startToPlayer - startPoint) / (startToEnd - startPoint))) * DistanceScale;
    }

    private IEnumerator CalculateProximalPenalty()
    {
        yield return new WaitForSeconds(PenaltyRateTimeThreshold);

        var currentTime = 0.0f;

        while (currentTime <= MaxPenaltyRateTime + PenaltyRateTimeThreshold)
        {
            CurrentPenalty += ProximalAnxietyPenaltyRate;
            yield return new WaitForSeconds(PenaltyTimeInterval);
            currentTime += PenaltyTimeInterval;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(nameof(CalculateProximalPenalty));
            _player = other.gameObject;
            SanityManager.Instance.AnxietyManager.AddProximalSource(this);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopCoroutine(nameof(CalculateProximalPenalty));
            CurrentPenalty = 0.0f;
            _player = null;
            SanityManager.Instance.AnxietyManager.RemoveProximalSource(this);
        }
    }

    private void OnDrawGizmos()
    {
        if (_player != null)
        {
            var dirTowardsPlayer = (_player.transform.position - gameObject.transform.position).normalized;
            var distance = Vector3.Distance(gameObject.transform.position + dirTowardsPlayer * ProximalSourceOffset, _player.transform.position);
            // Gizmos.DrawSphere(gameObject.transform.position + dirTowardsPlayer * ProximalSourceOffset, 0.1f);
            Gizmos.DrawSphere(gameObject.transform.position + dirTowardsPlayer * ProximalSourceOffset, 0.1f);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(gameObject.transform.position + dirTowardsPlayer * _proximalArea.radius, 0.1f);
        }
    }
}
