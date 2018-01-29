// Allan Murillo : Unity RPG Core Test Project
using UnityEngine;


public class Projectile : MonoBehaviour {


    public float speed;
    public float damage;



    public void SetDamage(float dmg)
    {
        damage = dmg;
    }

    void OnCollisionEnter(Collision other)
    {
        Component damageableComponent = other.gameObject.GetComponent(typeof(IDamageable));
        if (damageableComponent)
        {
            (damageableComponent as IDamageable).TakeDamage(damage);
        }
        Destroy(gameObject, .1f);
    }
} 