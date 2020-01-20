using RPG.Core;
using RPG.Stats;
using RPG.Saving;
using UnityEngine;
using RPG.Movement;
using RPG.Attributes;
using GameDevTV.Utils;
using System.Collections.Generic;


namespace RPG.Combat
{
    [RequireComponent(typeof(Animator))]
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        #region Fighter Class Members                
        [SerializeField] WeaponConfig defaultWeapon = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Transform rightHandTransform = null;

        //  Cached Variables
        Health target;
        Animator myAnimator;
        CharacterMove myCharacterMove;

        float timeSinceLastAttack = Mathf.Infinity;
        LazyValue<WeaponConfig> currentWeapon;
        #endregion


        private void Awake()
        {
            currentWeapon = new LazyValue<WeaponConfig>(SetupDefaultWeapon);
            myCharacterMove = GetComponent<CharacterMove>();
            myAnimator = GetComponent<Animator>();
        }

        private void Start()
        {
            currentWeapon.ForceInit();
        }

        private void Update()
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

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public static bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        private bool GetIsInRange()
        {   //  TODO : be able to differ melee from ranged and apply extra stats range
            return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.value.GetWeaponRange();
        }

        private WeaponConfig SetupDefaultWeapon()
        {
            AttachWeapon(defaultWeapon);
            return defaultWeapon;
        }

        public void EquipWeapon(WeaponConfig weapon)
        {
            currentWeapon.value = weapon;
            AttachWeapon(weapon);
        }

        private void AttachWeapon(WeaponConfig weapon)
        {
            weapon.SpawnWeapon(rightHandTransform, leftHandTransform, myAnimator);
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if (!(timeSinceLastAttack >= currentWeapon.value.GetWeaponFireRate())) return;
            TriggerAttack();
            timeSinceLastAttack = 0f;
        }

        private void TriggerAttack()
        {
            //  This will trigger the Animation Event : Hit()
            myAnimator.ResetTrigger("StopAttack");
            myAnimator.SetTrigger("Attack");
        }

        private void StopAttack()
        {
            myAnimator.ResetTrigger("Attack");
            myAnimator.SetTrigger("StopAttack");
        }

        #region Animation Events

        private void Hit()
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

        private void Shoot()
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
            WeaponConfig weapon = UnityEngine.Resources.Load<WeaponConfig>(weaponName);
            EquipWeapon(weapon);
        }   //  ISaveable                
        #endregion
    }
}