using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour, IWeapon
{
    private WeaponSO weaponData;

    public void Initialize(WeaponSO weaponData)
    {
        this.weaponData = weaponData;
    }

    public void Shoot()
    {
        // Implement shooting logic based on data properties
    }

    public void AimDownSights()
    {
        // Implement aiming down sights logic based on data properties
    }

    // Implement other IWeapon interface methods based on data properties
}
