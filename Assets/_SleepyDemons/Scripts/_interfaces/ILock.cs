using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILock
{
    public bool IsLocked { get; }
    public bool IsBroken { get;  }
    void SwitchLock();

    void BreakLock();
}
