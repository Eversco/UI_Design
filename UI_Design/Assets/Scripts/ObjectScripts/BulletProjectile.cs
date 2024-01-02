using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] protected LayerMask ignoreLayer = new LayerMask();
    [SerializeField] protected Transform vfxHit;
    [SerializeField] protected float maxTravelDistance = 500f;
    [SerializeField] protected float bulletSpeed = 30f;

    protected Rigidbody bulletRigidbody;
    protected float distanceTraveled;
    
    

    protected virtual void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        bulletRigidbody.velocity = transform.forward * bulletSpeed;
        distanceTraveled = 0f;
    }
    protected virtual void Update()
    {
        distanceTraveled += Time.deltaTime * bulletSpeed;
        if(distanceTraveled >= maxTravelDistance)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == ignoreLayer)
        {
            return;
        }

        if(other.gameObject.TryGetComponent(out ThirdPersonShooterController thirdPersonShooterController))
        {
            return;
        }
        if (other.gameObject.TryGetComponent(out BulletProjectile bulletProjectile))
        {
            return;
        }
        //Debug.Log("Hit " + other.gameObject.ToString() + "; Hit at" + transform.position.ToString());
        //Transform hitVfx = Instantiate(vfxHit, transform.position, Quaternion.identity);
        //hitVfx.localScale = new Vector3(2,2,2);
        Destroy(gameObject);
    }
}
