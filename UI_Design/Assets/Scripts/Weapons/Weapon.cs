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
    private Weapon instantiatedWeapon;
    public int ammo;
    public float shootCooldown;

    private void Update()
    {
        shootCooldown -= Time.deltaTime;
        if (shootCooldown < 0f) { shootCooldown = 0f; }
    }
    public virtual void Shoot(Vector3 mouseWorldPosition, Transform spawnGunPosition, Transform pfBulletProjectile)
    {
        ammo -= 1;
        shootCooldown = 1f / weaponData.firerate;

        
        // Implement shooting logic based on data properties
        //Vector3 actualMuzzlePosition = transform.position + transform.rotation * muzzlePosition.position;
        Vector3 aimDir = (mouseWorldPosition - muzzlePosition.position).normalized;
        

        Instantiate(pfBulletProjectile, muzzlePosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
        
    }

    public float GetShootCooldown()
    {
        return shootCooldown;
    }
    public bool CanShoot()
    {
        return ammo > 0 && shootCooldown <= 0f;
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

    public void Unequip()
    {
        gameObject.SetActive(false);
        //Destroy(spawnedWeapon);
    }

    public void Equip()
    {
        gameObject.SetActive(true);
        /*
        spawnedWeapon = Instantiate(gameObject, parent);
        instantiatedWeapon = spawnedWeapon.GetComponent<Weapon>();
        //spawnedWeapon.transform.rotation = Quaternion.Euler(0, parent.rotation.eulerAngles.y -90, -parent.rotation.eulerAngles.x);
        spawnedWeapon.transform.rotation *= Quaternion.Euler(0, -90, 0);
        Debug.Log(spawnedWeapon);
        */
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
