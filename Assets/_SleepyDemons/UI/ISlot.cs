using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISlot
{
    string GUID { get; }
    bool IsEmpty { get; }
}
