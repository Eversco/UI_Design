using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPJBullet : BulletProjectile
{
    public float damage;
    protected override void Start()
    {
        base.Start();
        transform.rotation *= Quaternion.Euler(new Vector3(0, -90, 0));
        
    }
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == ignoreLayer)
        {
            return;
        }

        if (other.gameObject.TryGetComponent(out ThirdPersonShooterController thirdPersonShooterController))
        {
            return;
        }
        if (other.gameObject.TryGetComponent(out BulletProjectile bulletProjectile))
        {
            return;
        }
        Debug.Log("Hit " + other.gameObject.ToString() + "; Hit at" + transform.position.ToString());
        Transform hitVfx = Instantiate(vfxHit, transform.position, Quaternion.identity);
        hitVfx.localScale = new Vector3(5,5,5);
        ApplyKnockback(transform.position, 5, damage);
        Destroy(gameObject);
    }

    private void ApplyKnockback(Vector3 explosionPoint, float radius, float force)
    {
        Collider[] colliders = Physics.OverlapSphere(explosionPoint, radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(force, explosionPoint, radius, 1.0f, ForceMode.Impulse);
            }
            IDamagable damagable = hit.GetComponent<IDamagable>();
            if(damagable != null)
            {
                float distanceMultiplier = (Vector3.Magnitude(hit.transform.position - explosionPoint) / radius) * (Vector3.Magnitude(hit.transform.position - explosionPoint) / radius);
                damagable.Damage(damage * distanceMultiplier);
            }
        }
    }
}
