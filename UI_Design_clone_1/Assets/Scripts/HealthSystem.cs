using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour, IDamagable
{
    // Start is called before the first frame update
    [SerializeField] private float maxHp = 100;
    [SerializeField] private float currentHp;
    void Start()
    {
        currentHp = maxHp;
    }
    public float GetHP()
    {
        return currentHp; 
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void Damage(float damage)
    {
        currentHp -= damage;
        Debug.Log(gameObject.ToString() + "Took " + damage.ToString() + "damage; Remaining HP: " + currentHp.ToString());
    }
}
