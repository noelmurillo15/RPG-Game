using RPG.Core;
using UnityEngine;


public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 1f;

    Health target = null;
    float damage = 0f;


    void Update()
    {
        if (target == null) return;
        transform.LookAt(GetAimLocation());
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Health>() != target) return;
        target.TakeDamage(damage);
        Destroy(gameObject);
    }

    public void SetTarget(Health _target, float _damage)
    {
        this.target = _target;
        this.damage = _damage;
    }

    Vector3 GetAimLocation()
    {
        CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
        if (targetCapsule == null) { return target.transform.position; }
        return target.transform.position + Vector3.up * targetCapsule.height * 0.5f;
    }
}