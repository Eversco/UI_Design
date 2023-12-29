using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageParent : MonoBehaviour, IDamagable
{
    //Can refactor
    private HealthSystem damagableParent;
    public void Damage(float damage)
    {
        if(damagableParent != null)
        {
            //Debug.Log(damagableParent.GetHP());
            damagableParent.Damage(damage);
        }
        else
        {
            Debug.LogWarning("Could not get damagable parent!");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        damagableParent = GetComponentInParent<HealthSystem>();
        Debug.Log(damagableParent);
    }
}
