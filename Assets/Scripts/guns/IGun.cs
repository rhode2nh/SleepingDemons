using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGun
{
    public void CheckChamber(bool newState);
    public void LoadBullet();
    public void EmptyChamber();
    public void Attack(bool newState);
    public void Aim(bool newState);
}
