using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RatAnimController : MonoBehaviour
{
    [SerializeField] private float _animationSpeed = 1.0f;
    
    Animator _animator;
    private NavMeshAgent _navMeshAgent;
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        _animator.speed = _navMeshAgent.velocity.magnitude * _animationSpeed;
        _animator.SetFloat("move", _navMeshAgent.velocity.magnitude);
    }
}
