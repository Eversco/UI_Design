using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]private WeaponSO weaponData;
    
    
    private GameObject spawnedWeapon;
    private CinemachineVirtualCamera aimVirtualCamera;
    private CinemachineVirtualCamera normalVirtualCamera;

    public virtual void Shoot()
    {
        // Implement shooting logic based on data properties
    }

    public void AimDownSights()
    {
        aimVirtualCamera.gameObject.SetActive(true);
        
    }
    public void ExitADS()
    {
        aimVirtualCamera.gameObject.SetActive(false);
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
        
        //spawnedWeapon.transform.rotation = Quaternion.Euler(0, parent.rotation.eulerAngles.y -90, -parent.rotation.eulerAngles.x);
        spawnedWeapon.transform.rotation *= Quaternion.Euler(0, -90, 0);
        Debug.Log(parent.localRotation);

    }

    public float GetZoom()
    {
        return weaponData.zoomMultiplier;
    }

    public void Initialize(CinemachineVirtualCamera aimVirtualCamera, CinemachineVirtualCamera normalVirtualCamera)
    {
        this.aimVirtualCamera = aimVirtualCamera;
        this.normalVirtualCamera = normalVirtualCamera;
        Debug.Log(normalVirtualCamera.m_Lens.FieldOfView / GetZoom());
        aimVirtualCamera.m_Lens.FieldOfView = normalVirtualCamera.m_Lens.FieldOfView / GetZoom();
    }






    // Implement other IWeapon interface methods based on data properties
}
