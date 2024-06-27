using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEquippable
{
    public void Attack(bool newState);
    public void CheckChamber(bool newState);
    public void CheckMagazine(bool newState);
    public void Aim(bool newState);
    public void LoadBullet();
    public void EmptyChamber();
    public void SwitchToChamber(bool newState);
    public void SwitchToMag(bool newState);
    public Vector3 GetEquipPos();
}
