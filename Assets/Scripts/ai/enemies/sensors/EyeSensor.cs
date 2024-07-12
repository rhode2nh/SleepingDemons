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
    [SerializeField] private float dotProductLosThreshold;
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
                    raycastDistance))
            {
                if (
                    hit.transform.CompareTag("Player")
                    && Vector3.Dot((_player.transform.position + playerOffset - transform.position).normalized, transform.forward) > dotProductLosThreshold
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
        Debug.DrawRay(transform.position, (transform.right * fov + transform.forward).normalized * raycastDistance, Color.green);
        Debug.DrawRay(transform.position, (-transform.right * fov + transform.forward).normalized * raycastDistance, Color.green);
        if (PlayerInTrigger)
        {
            Debug.DrawRay(transform.position, (_player.transform.position + playerOffset - transform.position).normalized * raycastDistance, Color.red);
        }
    }
}
