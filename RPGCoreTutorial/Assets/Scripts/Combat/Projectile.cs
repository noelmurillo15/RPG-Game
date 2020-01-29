using UnityEngine;
using RPG.Attributes;


namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 1f;
        [SerializeField] private bool isHoming = true;
        [SerializeField] private GameObject hitEffect = null;
        [SerializeField] private float maxLifeTime = 10f;
        [SerializeField] private GameObject[] destroyOnHit = null;
        [SerializeField] private float lifeAfterImpact = 2f;

        private Health _target = null;
        private GameObject _instigator = null;
        private float _totalDamage = 0f;


        private void Start()
        {
            transform.LookAt(GetAimLocation());
        }

        private void Update()
        {
            if (_target == null) return;
            if (isHoming && !_target.IsDead()) transform.LookAt(GetAimLocation());
            transform.Translate(Vector3.forward * (speed * Time.deltaTime));
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != _target) return;
            if (_target.IsDead()) return;

            _target.TakeDamage(_instigator, _totalDamage);
            speed = 0f;

            if (hitEffect != null)
            {
                Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            }

            foreach (GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }

            Destroy(gameObject, lifeAfterImpact);
        }

        public void SetTarget(Health target, GameObject instigator, float damage)
        {
            _target = target;
            _totalDamage = damage;
            _instigator = instigator;
            Destroy(gameObject, maxLifeTime);
        }   //  Damage is calculated from weapon Scriptable dmg + character base damage

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = _target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null) { return _target.transform.position; }
            return _target.transform.position + Vector3.up * (targetCapsule.height * 0.5f);
        }
    }
}