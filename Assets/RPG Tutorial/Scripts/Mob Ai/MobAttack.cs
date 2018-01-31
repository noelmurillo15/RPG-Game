// Allan Murillo : Unity RPG Core Test Project
using UnityEngine;


namespace RPG
{
    [RequireComponent(typeof(MobMaster))]
    [RequireComponent(typeof(MobStats))]
    public class MobAttack : MonoBehaviour
    {

        private MobMaster mobMaster;
        private MobStats mobStats;
        private Transform myTarget;
        private Transform myTransform;

        [SerializeField] float attackRate = 1;
        [SerializeField] float attackRange = 8f;
        private float nextAttack;



        void Initialize()
        {
            attackRate = Random.Range(2.25f, 3.25f);
            mobMaster = GetComponent<MobMaster>();
            mobStats = GetComponent<MobStats>();
            myTransform = transform;
        }

        void Update()
        {
            TryAttack();
        }

        void OnEnable()
        {
            Initialize();
            mobMaster.EventMobDie += DisableThis;
            mobMaster.EventMobSetNavTarget += SetAttackTarget;
        }

        void OnDisable()
        {
            mobMaster.EventMobDie -= DisableThis;
            mobMaster.EventMobSetNavTarget -= SetAttackTarget;
        }

        void SetAttackTarget(Transform target)
        {
            myTarget = target;
        }

        void TryAttack()
        {
            //Debug.Log("Attack Sequence Initialized");
            if (myTarget != null)
            {
                //Debug.Log("Target Locked");
                if (Time.time > nextAttack && !mobMaster.IsCriticallyHit)
                {
                    //Debug.Log("Preparing an attack");
                    nextAttack = Time.time + attackRate;
                    if (Vector3.Distance(myTransform.position, myTarget.position) <= attackRange)
                    {
                        //Debug.Log("Attacking!");
                        Vector3 lookatVector = new Vector3(myTarget.position.x, myTransform.position.y, myTarget.position.z);
                        myTransform.LookAt(lookatVector);
                        mobMaster.CallEventMobAttack();
                        mobMaster.IsOnRoute = false;
                        mobMaster.IsAttacking = true;
                    }
                }
            }
        }

        //  TODO : Attack animation must have event to use this function
        void OnEnemyAttack()
        {
            if (myTarget != null)
            {
                mobMaster.IsAttacking = false;
                mobMaster.CallEventMobExpUp(1);
                int damageToApply = mobStats.PhysicalDamage;
                if (Random.Range(0, 100) < mobStats.CriticalRate)
                {
                    damageToApply += mobStats.PhysicalDamage + mobStats.CriticalDamage;
                }

                if (myTarget.GetComponent<HealthSystem>() != null)
                {
                    myTarget.GetComponent<HealthSystem>().TakeDamage(damageToApply);
                    return;
                }
            }
        }

        void DisableThis()
        {
            this.enabled = false;
        }
    }
}