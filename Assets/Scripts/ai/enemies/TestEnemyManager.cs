using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyManager : MonoBehaviour, IHittable
{
    [SerializeField] private float health;
    [SerializeField] private float staggerThreshold;
    
    [SerializeField] public List<GameObject> waypoints;
    [SerializeField] public GameObject currentWaypoint;
    [SerializeField] public GameObject lastWaypoint;

    [SerializeField] private Animator animator;

    private float _currentStaggerValue;
    public void TakeDamage(float damage, Vector3 force, Vector3 torque)
    {
        // health -= damage;
        _currentStaggerValue += damage;
        if (_currentStaggerValue >= staggerThreshold)
        {
            animator.SetTrigger("isStaggered");
            staggerThreshold = 0.0f;
        }
        
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        animator.SetTrigger("isDying");       
    }
}
