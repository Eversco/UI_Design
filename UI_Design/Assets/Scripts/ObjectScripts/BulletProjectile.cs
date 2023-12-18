using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] private LayerMask ignoreLayer = new LayerMask();
    [SerializeField] private Transform vfxHit;
    [SerializeField] private float maxTravelDistance = 500f;
    [SerializeField] private float bulletSpeed = 30f;

    private Rigidbody bulletRigidbody;
    private float distanceTraveled;
    

    private void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
        bulletRigidbody.velocity = transform.forward * bulletSpeed;
        
        distanceTraveled = 0f;
    }
    private void Update()
    {
        distanceTraveled += Time.deltaTime * bulletSpeed;
        if(distanceTraveled >= maxTravelDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
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
        Debug.Log("Hit " + other.gameObject.ToString() + "; Hit at" + transform.position.ToString());
        //Transform hitVfx = Instantiate(vfxHit, transform.position, Quaternion.identity);
        //hitVfx.localScale = new Vector3(2,2,2);
        Destroy(gameObject);
    }
}
