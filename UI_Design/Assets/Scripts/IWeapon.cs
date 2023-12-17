using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    void Shoot();
    void AimDownSights();
    public string GetWeaponName();
    public void Unequip(Transform parent);
    public void Equip(Transform parent);
}

