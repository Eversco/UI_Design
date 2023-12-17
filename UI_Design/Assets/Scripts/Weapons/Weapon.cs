using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour, IWeapon
{
    [SerializeField]private WeaponSO weaponData;
    private GameObject spawnedWeapon;
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

    public string GetWeaponName()
    {
        return weaponData.name;
    }

    public void Unequip(Transform parent)
    {
        Destroy(spawnedWeapon);
    }

    public void Equip(Transform parent)
    {
        spawnedWeapon = Instantiate(gameObject, parent);
        //spawnedWeapon.transform.position = new Vector3(0.331f, 0.934f, 1.102);
        spawnedWeapon.transform.rotation = Quaternion.Euler(parent.rotation.eulerAngles + new Vector3(0, -90, 0));
    }

    




    // Implement other IWeapon interface methods based on data properties
}
