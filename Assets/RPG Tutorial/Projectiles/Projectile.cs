// Allan Murillo : Unity RPG Core Test Project
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Projectile : MonoBehaviour {


    public float projectileSpeed;
    public float projectileDamage;



    void OnTriggerEnter(Collider other)
    {
        Component damageableComponent = other.gameObject.GetComponent(typeof(IDamageable));
        //Debug.Log("Projectile Hit : " + damageableComponent);
        if (damageableComponent)
        {
            (damageableComponent as IDamageable).TakeDamage(projectileDamage);
        }
    }
} 