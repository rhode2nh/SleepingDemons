using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem
{
    public string GUID { get; }
    public void Use();
}
