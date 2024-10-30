using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.Serialization;

public class EyeSensor : MonoBehaviour
{
    [SerializeField] private float raycastDistance;
    [SerializeField] private float timeToAcknowledgePlayer;
    [SerializeField] private float losePlayerThresholdTimer;
    [SerializeField] private float fov;
    [SerializeField, Range(0, 360)] private float dotProductLosThresholdInDegrees;
    [SerializeField] Vector3 playerOffset;

    [field: SerializeField, ReadOnly] public bool IsLookingAtPlayer { get; private set; }
    [field: SerializeField, ReadOnly] public bool HasAcknowledgedPlayer { get; private set; }
    [field: SerializeField, ReadOnly] public bool HasLostPlayer { get; set; }
    [field: SerializeField, ReadOnly] public bool PlayerInTrigger { get; set; }

    [SerializeField, ReadOnly] private float currentTimer;
    [SerializeField, ReadOnly] private float currentLostTimer;

    private GameObject _player;

    void FixedUpdate()
    {
        Sense();
    }

    float ConvertToDegrees(float num)
    {
        float slope = 1.0f * ((1.0f - -1.0f) / (360.0f - 0.0f));
        return 1 - (0.0f + slope * (num - 0.0f));
    }

    void Sense()
    {
        if (IsLookingAtPlayer)
        {
            currentTimer += Time.deltaTime;
            currentLostTimer = 0.0f;
            if (currentTimer >= timeToAcknowledgePlayer)
            {
                HasAcknowledgedPlayer = true;
            }
        }
        else
        {
            currentTimer = 0.0f;
        }

        if (HasAcknowledgedPlayer && !IsLookingAtPlayer)
        {
            if (currentLostTimer >= losePlayerThresholdTimer)
            {
                HasAcknowledgedPlayer = false;
                HasLostPlayer = true;
                currentLostTimer = 0.0f;
            }
            else
            {
                HasLostPlayer = false;
                currentLostTimer += Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInTrigger = true;
            _player = other.gameObject;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (PlayerInTrigger && _player != null)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, _player.transform.position + playerOffset - transform.position, out hit,
                    (_player.transform.position + playerOffset - transform.position).magnitude))
            {
                if (
                    hit.transform.CompareTag("Player")
                    && Vector3.Dot((_player.transform.position + playerOffset - transform.position).normalized, transform.forward) > ConvertToDegrees(dotProductLosThresholdInDegrees)
                    )
                {
                    IsLookingAtPlayer = true;
                }
                else
                {
                    IsLookingAtPlayer = false;
                }
            }
            else
            {
                IsLookingAtPlayer = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInTrigger = false;
            IsLookingAtPlayer = false;
            _player = null;
        }
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, transform.forward * raycastDistance, Color.green);
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(-dotProductLosThresholdInDegrees / 2.0f, Vector3.up) * transform.forward * raycastDistance, Color.green);
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(dotProductLosThresholdInDegrees / 2.0f, Vector3.up) * transform.forward * raycastDistance, Color.green);
        if (PlayerInTrigger)
        {
            Debug.DrawRay(transform.position, (_player.transform.position + playerOffset - transform.position), Color.red);
        }
    }
}
