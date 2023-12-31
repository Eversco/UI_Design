using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Weapon : MonoBehaviour
{
    [SerializeField] protected WeaponSO weaponData;
    [SerializeField] protected Transform muzzlePosition;
    
    [SerializeField] protected float shootCooldown;
    [SerializeField] protected float reloadCooldown;
    public bool isReloading { get; protected set; }
    public int ammo { get; protected set; }

    protected GameObject weaponWielder;
    protected CinemachineVirtualCamera aimVirtualCamera;
    protected CinemachineVirtualCamera normalVirtualCamera;
    protected Animator weaponAnimator;
    protected int semiFireDebounce; 
    public string weaponName { get; [SerializeField] private set; }

    protected const string SHOOT = "Shoot";

    private void Awake()
    {
        weaponAnimator = GetComponent<Animator>();
        weaponName = weaponData.name;
    }
    private void Update()
    {
        shootCooldown -= Time.deltaTime;
        reloadCooldown -= Time.deltaTime;
        if (shootCooldown < 0f) { shootCooldown = 0f; }
        if (reloadCooldown < 0f) 
        { 
            reloadCooldown = 0f;
            if(isReloading)
            {
                RefillAmmo();
            }
        }
    }
    public virtual void Shoot(Vector3 mouseWorldPosition, Transform pfBulletProjectile, float rayDistance, LayerMask targetLayer)
    {
        semiFireDebounce -= 1;
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
                Debug.Log("Hit " + rayCastHit.transform.gameObject.ToString() + "; Hit at" + rayCastHit.point.ToString());
                HandleKnockback(rayCastHit, ray, weaponData.damage);

                if (rayCastHit.transform.TryGetComponent(out IDamagable target))
                {
                    target.Damage(weaponData.damage);
                }
                /*
                if (rayCastHit.transform.GetComponent<PlayerMorphed>() == null && rayCastHit.transform.GetComponent<MorphTarget>() != null)
                {
                    //player hit a morphable target that is not a player, PUNISH THEM.
                    IDamagable weaponWielderDamagable = weaponWielder.GetComponent<IDamagable>();
                    if (weaponWielderDamagable != null)
                    {
                        weaponWielderDamagable.Damage(weaponData.damage * weaponData.penaltyMultiplier);
                    }
                    else
                    {
                        Debug.LogWarning(weaponWielder.ToString() + "does not have a healthsystem with" + weaponWielderDamagable.GetType().Name + "implemented!");
                    }
                }
                */
               

            }
            Transform bullet = Instantiate(pfBulletProjectile, muzzlePosition.position, Quaternion.LookRotation(aimDir, Vector3.up) * Quaternion.Euler(new Vector3(x, y, 0)));


            //bullet.localScale = new Vector3(0.7f, 0.7f, 1f);
        }   
    }
    public void StopShoot()
    {
        semiFireDebounce = 1;
    }
    public void HandleKnockback(RaycastHit rayCastHit, Ray ray, float damage)
    {
        Rigidbody body = rayCastHit.rigidbody;
        if (body == null || body.isKinematic) { return; }
        body.AddForce(damage * 1f * ray.direction, ForceMode.Impulse);
    }
    public float GetShootCooldown()
    {
        return shootCooldown;
    }
    public bool CanShoot()
    {
        return ammo > 0 && shootCooldown <= 0f && reloadCooldown <= 0f && !(weaponData.fireType == FireType.Semi && semiFireDebounce <= 0);
    }
    public bool CanReload()
    {
        return !isReloading && ammo < weaponData.clipSize;
    }

    public void AimDownSights()
    {
        aimVirtualCamera.gameObject.SetActive(true);
        
    }
    public void ExitADS()
    {
        aimVirtualCamera.gameObject.SetActive(false);
    }
    public void Reload()
    {
        reloadCooldown = weaponData.reloadTime;
        isReloading = true;
    }
    public virtual void RefillAmmo()
    {
        isReloading = false;
        ammo = weaponData.clipSize;
    }
    public string GetWeaponName()
    {
        return weaponData.name;
    }

    public void Unequip()
    {
        isReloading = false;
        reloadCooldown = 0f;
        gameObject.SetActive(false);
        semiFireDebounce = 1;
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
    

    public void InitializeGunStats(GameObject weaponWielder) //added parameter wielding to seeker itself because of the need to damage self from here
    {
        ammo = weaponData.clipSize;
        shootCooldown = 0f;
        reloadCooldown = 0f;
        semiFireDebounce = 1;
        isReloading = false;
        this.weaponWielder = weaponWielder;
    }
    // Implement other IWeapon interface methods based on data properties
}
