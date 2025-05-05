using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LevelBounds : MonoBehaviour
{
    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnDrawGizmos()
    {
        if (_collider == null)
        {
            _collider = GetComponent<Collider>();
        }
        Gizmos.color = Color.red;
        Gizmos.DrawRay(_collider.bounds.center, _collider.transform.forward * 0.2f);
        Gizmos.DrawWireCube(_collider.bounds.center, _collider.bounds.size);
    }
}
