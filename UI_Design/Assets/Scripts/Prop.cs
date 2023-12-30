using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Prop : MonoBehaviour, IDamagable
{
    [SerializeField] private PropSO propData;
    [SerializeField] private float currentHP;
    public void Damage(float damage)
    {
        currentHP -= damage;
        if(currentHP <= 0)
        {
            Die();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHP = propData.maxHP;
        Rigidbody rigidBody = GetComponent<Rigidbody>();
        if(rigidBody == null)
        {
            Debug.LogWarning(gameObject.ToString() + "does not have RigidBody!");
        }
        else 
        {
            rigidBody.mass = propData.mass;
        }   
    }

    // Update is called once per frame
    protected virtual void Die()
    {
        Debug.Log(gameObject.ToString() + " died a horrible death");
        Instantiate(propData.vfxDie, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
