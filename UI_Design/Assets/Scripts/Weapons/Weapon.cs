using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]private WeaponSO weaponData;
    [SerializeField]private Transform muzzlePosition;


    private GameObject spawnedWeapon;
    private CinemachineVirtualCamera aimVirtualCamera;
    private CinemachineVirtualCamera normalVirtualCamera;
    private int ammo;
    private float shootCooldown;
    private int num = 0;

    void Update()
    {
        
        
    }
    public virtual void Shoot(Vector3 mouseWorldPosition, Transform spawnGunPosition, Transform pfBulletProjectile)
    {
        ammo -= 1;
        shootCooldown = 1f / weaponData.firerate;

        Debug.Log(shootCooldown); 
        // Implement shooting logic based on data properties
        Vector3 actualMuzzlePosition = spawnedWeapon.transform.position + spawnedWeapon.transform.rotation * muzzlePosition.position;
        Vector3 aimDir = (mouseWorldPosition - actualMuzzlePosition).normalized;
        

        Instantiate(pfBulletProjectile, actualMuzzlePosition, Quaternion.LookRotation(aimDir, Vector3.up));
        
    }
    public bool CanShoot()
    {
        return ammo > 0;
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
        Debug.Log(spawnedWeapon);
        
        

    }

    public float GetZoom()
    {
        return weaponData.zoomMultiplier;
    }

    public void InitializeCamera(CinemachineVirtualCamera aimVirtualCamera, CinemachineVirtualCamera normalVirtualCamera)
    {
        this.aimVirtualCamera = aimVirtualCamera;
        this.normalVirtualCamera = normalVirtualCamera;
        //Debug.Log(normalVirtualCamera.m_Lens.FieldOfView / GetZoom());
        aimVirtualCamera.m_Lens.FieldOfView = normalVirtualCamera.m_Lens.FieldOfView / GetZoom();
    }

    public void InitializeGunStats()
    {
        ammo = weaponData.clipSize;
        shootCooldown = 0f;
    }
    // Implement other IWeapon interface methods based on data properties
}
