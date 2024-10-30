using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHittable
{
    public void TakeDamage(float damage, Vector3 force, Vector3 torque);
}
