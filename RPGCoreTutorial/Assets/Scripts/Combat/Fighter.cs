﻿using RPG.Core;
using RPG.Stats;
using UnityEngine;
using RPG.Movement;


namespace RPG.Combat
{
    [RequireComponent(typeof(Animator))]
    public class Fighter : MonoBehaviour, IAction
    {
        #region Fighter Class Members        
        [SerializeField] Weapon currentWeapon = null;
        [SerializeField] Weapon defaultWeapon = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Transform rightHandTransform = null;

        //  Cached Variables
        Health target;
        Animator myAnimator;
        CharacterMove myCharacterMove;
        CharacterStats myCharacterStats;

        float timeSinceLastAttack = Mathf.Infinity;
        #endregion


        void Start()
        {
            myAnimator = GetComponent<Animator>();
            myCharacterMove = GetComponent<CharacterMove>();
            myCharacterStats = GetComponent<CharacterStats>();
            EquipWeapon(defaultWeapon);
        }

        void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;
            if (target.IsDead()) return;

            if (!GetIsInRange())
            {   //  While fighting we want fighter to move at max speed
                myCharacterMove.MoveTo(target.transform.position, 1f);
            }
            else
            {
                myCharacterMove.Cancel();
                AttackBehaviour();
            }
        }

        public void Attack(GameObject _combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = _combatTarget.GetComponent<Health>();
        }

        public bool CanAttack(GameObject _combatTarget)
        {
            if (_combatTarget == null) return false;
            Health targetToTest = _combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        bool GetIsInRange()
        {   //  TODO : be able to differ melee from ranged and apply extra stats range
            return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.GetWeaponRange();
        }

        public void EquipWeapon(Weapon _weapon)
        {
            currentWeapon = _weapon;
            currentWeapon.SpawnWeapon(rightHandTransform, leftHandTransform, myAnimator);
        }

        void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack >= currentWeapon.GetTimeBetweenAttacks())
            {
                TriggerAttack();
                timeSinceLastAttack = 0f;
            }
        }

        void TriggerAttack()
        {
            //  This will trigger the Animation Event : Hit()
            myAnimator.ResetTrigger("StopAttack");
            myAnimator.SetTrigger("Attack");
        }

        void StopAttack()
        {
            myAnimator.ResetTrigger("Attack");
            myAnimator.SetTrigger("StopAttack");
        }

        #region Animation Events
        void Hit()
        {
            if (target == null) return;

            if (currentWeapon.HasProjectile())
            {   //  Ranged Attack
                currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target, myCharacterStats.GetMagicalAttack());
            }
            else
            {   //  Melee Attack
                target.TakeDamage(currentWeapon.GetWeaponDamage() + myCharacterStats.GetPhysicalAttack());
            }
        }

        void Shoot()
        {
            Hit();
        }
        #endregion                

        #region Interface Methods
        public void Cancel()
        {
            StopAttack();
            target = null;
        }   //  IAction
        #endregion
    }
}