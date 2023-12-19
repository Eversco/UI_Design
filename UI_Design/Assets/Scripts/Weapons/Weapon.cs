using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponSO weaponData;
    [SerializeField] private Transform muzzlePosition;

    private GameObject spawnedWeapon;
    private CinemachineVirtualCamera aimVirtualCamera;
    private CinemachineVirtualCamera normalVirtualCamera;
    private Weapon instantiatedWeapon;
    private Animator weaponAnimator;
    public int ammo;
    public float shootCooldown;

    private const string SHOOT = "Shoot";

    private void Awake()
    {
        weaponAnimator = GetComponent<Animator>();
    }
    private void Update()
    {
        shootCooldown -= Time.deltaTime;
        if (shootCooldown < 0f) { shootCooldown = 0f; }
    }
    public virtual void Shoot(Vector3 mouseWorldPosition, Transform pfBulletProjectile, float rayDistance, LayerMask targetLayer)
    {
        ammo -= 1;
        shootCooldown = 1f / weaponData.firerate;

        weaponAnimator.SetTrigger(SHOOT);
        // Implement shooting logic based on data properties
        //Vector3 actualMuzzlePosition = transform.position + transform.rotation * muzzlePosition.position;
        Vector3 aimDir = (mouseWorldPosition - muzzlePosition.position).normalized;
        
        for(int i = 0; i < weaponData.pelletCount; i++)
        {
            float angle = Random.Range(0f, 2f * Mathf.PI);
            float distance = Mathf.Sqrt(Random.Range(0f, 1f)) * weaponData.inaccuracy;
            float x = distance * Mathf.Cos(angle);
            float y = distance * Mathf.Sin(angle);

            Ray ray = new Ray(muzzlePosition.position, Quaternion.Euler(new Vector3(x, y, 0)) * aimDir);
            if (Physics.Raycast(ray, out RaycastHit rayCastHit, rayDistance, targetLayer))
            {
                Instantiate(weaponData.vfxHit, rayCastHit.point, Quaternion.identity);
                Debug.Log("Hit " + rayCastHit.ToString() + "; Hit at" + rayCastHit.point.ToString());
                
            }
            Transform bullet = Instantiate(pfBulletProjectile, muzzlePosition.position, Quaternion.LookRotation(aimDir, Vector3.up) * Quaternion.Euler(new Vector3(x, y, 0)));

            //bullet.localScale = new Vector3(0.7f, 0.7f, 1f);
        }
        
        
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
