using RPG.Core;
using RPG.Stats;
using RPG.Saving;
using UnityEngine;
using RPG.Movement;
using RPG.Resources;
using GameDevTV.Utils;
using System.Collections.Generic;
using System;

namespace RPG.Combat
{
    [RequireComponent(typeof(Animator))]
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        #region Fighter Class Members                
        [SerializeField] Weapon defaultWeapon = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Transform rightHandTransform = null;

        //  Cached Variables
        Health target;
        Animator myAnimator;
        CharacterMove myCharacterMove;

        float timeSinceLastAttack = Mathf.Infinity;
        LazyValue<Weapon> currentWeapon;
        #endregion


        void Awake()
        {
            currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
            myCharacterMove = GetComponent<CharacterMove>();
            myAnimator = GetComponent<Animator>();
        }

        void Start()
        {
            currentWeapon.ForceInit();
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

        public Health GetTarget()
        {
            return target;
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
            return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.value.GetWeaponRange();
        }

        Weapon SetupDefaultWeapon()
        {
            AttachWeapon(defaultWeapon);
            return defaultWeapon;
        }

        public void EquipWeapon(Weapon _weapon)
        {
            currentWeapon.value = _weapon;
            AttachWeapon(_weapon);
        }

        void AttachWeapon(Weapon weapon)
        {
            weapon.SpawnWeapon(rightHandTransform, leftHandTransform, myAnimator);
        }

        void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack >= currentWeapon.value.GetWeaponFireRate())
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

            //  BaseStats.GetStat calculates the additive damage from any IModifierProvider and deals the total damage
            if (currentWeapon.value.HasProjectile())
            {   //  Ranged Attack
                currentWeapon.value.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, GetComponent<BaseStats>().GetStat(Stat.RANGE_DMG));
            }
            else
            {   //  Melee Attack
                target.TakeDamage(gameObject, GetComponent<BaseStats>().GetStat(Stat.MELEE_DMG));
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

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (stat == Stat.MELEE_DMG || stat == Stat.RANGE_DMG)
            {
                yield return currentWeapon.value.GetWeaponBaseDamage();
            }
        }   //  IModifierProvider

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.MELEE_DMG || stat == Stat.RANGE_DMG)
            {
                yield return currentWeapon.value.GetPercentageBonus();
            }
        }   //  IModifierProvider

        public object CaptureState()
        {
            print("Trying to get LazyValue<Weapon> Name !");
            return currentWeapon.value.name;
        }   //  ISaveable

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            Weapon weapon = UnityEngine.Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }   //  ISaveable                
        #endregion
    }
}