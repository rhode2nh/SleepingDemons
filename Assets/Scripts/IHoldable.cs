using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHoldable
{
    public void Hold(Vector3 hitPoint, bool isHolding);
}
