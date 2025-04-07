using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class LockController : MonoBehaviour
{
    private List<ILock> _locks = new();
    
    void Awake()
    {
        _locks = GetComponentsInChildren<ILock>().ToList();
    }

    public bool IsLocked()
    {
        return _locks.Count > 0 && _locks.Any(llock => llock.IsLocked);
    }
}
