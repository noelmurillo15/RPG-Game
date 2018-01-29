// Allan Murillo : Unity RPG Core Test Project
using UnityEngine;


public class Projectile : MonoBehaviour {


    public float speed;
    public float damage = 5f;



    public void SetDamage(float dmg)
    {
        damage = dmg;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            return;
        }

        Component damageableComponent = other.gameObject.GetComponent(typeof(RPG.IDamageable));
        if (damageableComponent)
        {
            (damageableComponent as RPG.IDamageable).AdjustHealth(damage * -1f);
        }
        Destroy(gameObject, .01f);
    }
} 