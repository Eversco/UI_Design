using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPJ : Weapon
{
    [SerializeField] private Transform rocket;
    public override void Shoot(Vector3 mouseWorldPosition, Transform pfBulletProjectile, float rayDistance, LayerMask targetLayer)
    {
        semiFireDebounce -= 1;
        ammo -= 1;
        shootCooldown = 1f / weaponData.firerate;

        weaponAnimator.SetTrigger(SHOOT);
        rocket.gameObject.SetActive(false);
        // Implement shooting logic based on data properties
        //Vector3 actualMuzzlePosition = transform.position + transform.rotation * muzzlePosition.position;
        Vector3 aimDir = (mouseWorldPosition - muzzlePosition.position).normalized;

        for (int i = 0; i < weaponData.pelletCount; i++)
        {
            float angle = Random.Range(0f, 2f * Mathf.PI);
            float distance = Mathf.Sqrt(Random.Range(0f, 1f)) * weaponData.inaccuracy;
            float x = distance * Mathf.Cos(angle);
            float y = distance * Mathf.Sin(angle);
            
            Transform bullet = Instantiate(weaponData.bullet, muzzlePosition.position, Quaternion.LookRotation(aimDir, Vector3.up)  * Quaternion.Euler(new Vector3(x, y, 0)));
            bullet.GetComponent<RPJBullet>().damage = weaponData.damage;
            //bullet.rotation *= Quaternion.Euler(new Vector3(0, -90, 0));

            //bullet.localScale = new Vector3(0.7f, 0.7f, 1f);
        }
    }
    public override void RefillAmmo()
    {
        base.RefillAmmo();
        rocket.gameObject.SetActive(true);
    }
}
