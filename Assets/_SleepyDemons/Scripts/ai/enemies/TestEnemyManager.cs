using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestEnemyManager : MonoBehaviour, IDamageable
{
    [SerializeField] public float health;
    [SerializeField] private float staggerThreshold;
    [SerializeField] private NavMeshAgent agent;
    
    [SerializeField] public List<GameObject> waypoints;
    [SerializeField] public GameObject safePoint;
    [SerializeField] public GameObject currentWaypoint;
    [SerializeField] public GameObject lastWaypoint;
    [SerializeField] public GameObject player;

    [SerializeField] private Animator animator;

    private float _currentStaggerValue;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
    }

    public void TakeDamage(float damage, Vector3 force, Vector3 torque)
    {
        health -= damage;
        _currentStaggerValue += damage;
        if (_currentStaggerValue >= staggerThreshold)
        {
            // animator.SetTrigger("isStaggered");
            staggerThreshold = 0.0f;
        }
        
        if (health <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (health <= 100)
        {
            health += amount;
        }

        if (health > 100)
        {
            health = 100;
        }
    }

    public void Test()
    {
        agent.SetDestination(safePoint.transform.position);
        Debug.Log(agent.pathPending);
    }

    private void Die()
    {
        // animator.SetTrigger("isDying");       
    }
}
