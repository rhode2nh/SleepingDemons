using System;
using UnityEngine;

public class RecursiveTeleporterManager : MonoBehaviour
{
    [SerializeField] protected Teleporter _source;
    [SerializeField] protected Teleporter _destination;
    [SerializeField] protected Teleporter _initialSourceDestination;

    [SerializeField] protected int _maxDepth;

    [SerializeField, ReadOnly] protected int _currentDepth = 1;

    protected virtual void Start()
    {
        _source.Init(OnSourceEnter, _initialSourceDestination);
        _destination.Init(OnDestinationEnter, _source);
    }

    protected virtual void OnSourceEnter()
    {
        if (_currentDepth > 0)
        {
            _currentDepth -= 1;
        }

        if (_currentDepth == 0)
        {
            _source.Destination = _initialSourceDestination;
        }
    }

    protected virtual void OnDestinationEnter()
    {
        if (_currentDepth < _maxDepth)
        {
            _currentDepth += 1;
        }

        if (_currentDepth > 0)
        {
            _source.Destination = _destination;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        if (_source == null || _destination == null) return;
        Gizmos.DrawLine(_source.transform.position, _destination.transform.position);
        if (_initialSourceDestination == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(_source.transform.position, _initialSourceDestination.transform.position);
    }
}
