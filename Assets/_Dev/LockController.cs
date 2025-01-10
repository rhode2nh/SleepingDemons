using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LockController : MonoBehaviour
{
    [SerializeField] GameObject _lockParent;
    private List<ILock> _locks = new();
    
    void Awake()
    {
        _locks = new List<ILock>(_lockParent.GetComponentsInChildren<ILock>());
    }

    public bool IsLocked()
    {
        if (_locks.Count <= 0)
        {
            return true;
        }
        return _locks.All(llock => llock.IsLocked);
    }
}
