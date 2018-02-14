/// <summary>
/// 2/13/18
/// Allan Murillo
/// RPG Core Project
/// MobAttack.cs
/// </summary>
using UnityEngine;


namespace RPG {

    public class MobAttack : MonoBehaviour {


        Character mobMaster;
        Transform lockedTarget;
        private float nextAttack;



        void Initialize()
        {
            lockedTarget = null;
            mobMaster = GetComponent<Character>();
        }        

        void OnEnable()
        {
            Initialize();
            mobMaster.EventCharacterDie += DisableThis;
            mobMaster.EventSetAttackTarget += SetAttackTarget;
        }

        void OnDisable()
        {
            mobMaster.EventCharacterDie -= DisableThis;
            mobMaster.EventSetAttackTarget -= SetAttackTarget;
        }

        void Update()
        {
            TryAttack();
        }

        #region Attack
        /// <summary>
        /// Keeps a reference of my Attack Target
        /// Called when Character Nav Target is Set
        /// </summary>
        /// <param name="target"></param>
        void SetAttackTarget(Transform target)
        {
            lockedTarget = target;
        }
        /// <summary>
        /// 
        /// </summary>
        void TryAttack()
        {
            //Debug.Log("Attack Sequence Initialized");
            if (lockedTarget != null)
            {
                //Debug.Log("Target Locked");
                if (Time.time > nextAttack && !mobMaster.IsCriticallyHit)
                {
                    //Debug.Log("Preparing an attack");
                    nextAttack = Time.time + mobMaster.AttackRate;
                    if (Vector3.Distance(mobMaster.MyTransformRef.position, lockedTarget.position) <= mobMaster.MeleeRange)
                    {
                        //Debug.Log("Attacking!");
                        Vector3 lookatVector = new Vector3(lockedTarget.position.x, mobMaster.MyTransformRef.position.y, lockedTarget.position.z);
                        mobMaster.MyTransformRef.LookAt(lookatVector);
                        mobMaster.CallEventCharacterAttack();
                        mobMaster.IsOnRoute = false;
                        mobMaster.IsAttacking = true;
                    }
                }
            }
        }
        /// <summary>
        /// Attack Animation MUST call OnEnemyAttack
        /// through the animation event system
        /// </summary>
        void OnEnemyAttack()
        {
            if (mobMaster.AttackTarget != null && lockedTarget == mobMaster.AttackTarget)
            {
                int damageToApply = mobMaster.PhysicalAttack;
                if (Random.Range(0, 100) < mobMaster.CriticalRate)
                {
                    damageToApply += damageToApply + mobMaster.CriticalDamage;
                }

                if (lockedTarget.GetComponent<HealthSystem>() != null)
                {
                    lockedTarget.GetComponent<HealthSystem>().TakeDamage(damageToApply);
                }
            }
            mobMaster.IsAttacking = false;
            mobMaster.AttackTarget = null;
            lockedTarget = null;
        }
        /// <summary>
        /// Disables Upon Death
        /// </summary>
        void DisableThis()
        {
            this.enabled = false;
        }
        #endregion
    }
}