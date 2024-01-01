using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [SerializeField] protected int health;
    [SerializeField] protected int maxHealth;

    [SerializeField] protected bool isDead;

    [SerializeField] protected int bullets;
    [SerializeField] protected int maxBullets;

    [SerializeField] protected bool noBullets;

    [SerializeField] protected string currentGun;

    [SerializeField] private PlayerHUD playerHUD;


    private void Start()
    {

        InitVariables();
        InitBullets();
        
    }

    #region Health Dep.

    public void CheckHealth()
    {
        if (health <= 0)
        {
            health = 0;
            isDead = true;
        }
        if (health >= maxHealth)
        {
            health = maxHealth;
        }
        
    }

    public void Die()
    {
        isDead = true;
    }

    public void SetHealthTo(int healthToSetTo)
    {
        health = healthToSetTo;
        CheckHealth();
        playerHUD.UpdateHealth(health);
    }

    public void TakeDamage(int damage)
    {
        int healAfterDamage = health - damage;
        SetHealthTo(healAfterDamage);
    }

    public void Heal(int heal)
    {
        int healthAfterHeal = health + heal;
        CheckHealth();
    }

    public void InitVariables()
    {
        maxHealth = 100;
        SetHealthTo(maxHealth);
        isDead = false;
    }
    #endregion

    #region Bullet Dep.
    
    public void CheckBullets()
    {
        if (bullets <= 0)
        {
            bullets = 0;
            noBullets = true;
        }
        if (bullets >= maxBullets)
        {
            bullets = maxBullets;
        }
        playerHUD.UpdateBullets(bullets, maxBullets);
    }
    
    public void CheckGun(Weapon currentWeapon)
    {
        currentGun = currentWeapon.name;
        maxBullets = currentWeapon.GetClipSize();
        bullets = currentWeapon.GetAmmo();
        playerHUD.UpdateBullets(bullets, maxBullets);
        //Debug.Log(currentGun);
    }

    public void Empty()
    {
        noBullets = true;
    }

    public void SetBulletsTo(int bulletsToSetTo)
    {
        bullets = bulletsToSetTo;
        CheckBullets();
    }


    public void ReduceBullet()
    {
        bullets--; // Decrement the bullet count
        CheckBullets(); // Check and update bullet status
        playerHUD.UpdateBullets(bullets, maxBullets); // Update the HUD
    }

    public void Reload(int reloadback)
    {
        bullets = reloadback;
    }

    public void Reloaded()
    {
        playerHUD.Reloading();
    }

    public void NotReloaded()
    {
        playerHUD.NotReloading();
        playerHUD.UpdateBullets(bullets, maxBullets);
    }

    public void InitBullets()
    {
        SetBulletsTo(maxBullets);
        noBullets = false;
    }
    #endregion

}
