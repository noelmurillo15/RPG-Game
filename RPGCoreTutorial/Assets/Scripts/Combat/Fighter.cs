using ANM.Attributes;
using GameDevTV.Utils;
using UnityEngine;
using System.Collections.Generic;
using ANM.Core;
using ANM.Movement;
using ANM.Saving;
using ANM.Stats;

namespace ANM.Combat
{
    [RequireComponent(typeof(Animator))]
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        #region Fighter Class Members                
        [SerializeField] private WeaponConfig defaultWeapon = null;
        [SerializeField] private Transform leftHandTransform = null;
        [SerializeField] private Transform rightHandTransform = null;

        private Health _target;
        private Animator _myAnimator;
        private CharacterMove _myCharacterMove;

        private float _timeSinceLastAttack = Mathf.Infinity;
        private LazyValue<WeaponConfig> _currentWeapon;
        private static readonly int StopAttack1 = Animator.StringToHash("StopAttack");
        private static readonly int Attack1 = Animator.StringToHash("Attack");
        #endregion


        private void Awake()
        {
            _currentWeapon = new LazyValue<WeaponConfig>(SetupDefaultWeapon);
            _myCharacterMove = GetComponent<CharacterMove>();
            _myAnimator = GetComponent<Animator>();
        }

        private void Start()
        {
            _currentWeapon.ForceInit();
        }

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;

            if (_target == null) return;
            if (_target.IsDead()) return;

            if (!GetIsInRange())
            {   //  While fighting we want fighter to move at max speed
                _myCharacterMove.MoveTo(_target.transform.position, 1f);
            }
            else
            {
                _myCharacterMove.Cancel();
                AttackBehaviour();
            }
        }

        public Health GetTarget()
        {
            return _target;
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            _target = combatTarget.GetComponent<Health>();
        }

        public static bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        private bool GetIsInRange()
        {   //  TODO : be able to differ melee from ranged and apply extra stats range
            return Vector3.Distance(transform.position, _target.transform.position) < _currentWeapon.value.GetWeaponRange();
        }

        private WeaponConfig SetupDefaultWeapon()
        {
            AttachWeapon(defaultWeapon);
            return defaultWeapon;
        }

        public void EquipWeapon(WeaponConfig weapon)
        {
            _currentWeapon.value = weapon;
            AttachWeapon(weapon);
        }

        private void AttachWeapon(WeaponConfig weapon)
        {
            weapon.SpawnWeapon(rightHandTransform, leftHandTransform, _myAnimator);
        }

        private void AttackBehaviour()
        {
            transform.LookAt(_target.transform);
            if (!(_timeSinceLastAttack >= _currentWeapon.value.GetWeaponFireRate())) return;
            TriggerAttack();
            _timeSinceLastAttack = 0f;
        }

        private void TriggerAttack()
        {
            //  This will trigger the Animation Event : Hit()
            _myAnimator.ResetTrigger(StopAttack1);
            _myAnimator.SetTrigger(Attack1);
        }

        private void StopAttack()
        {
            _myAnimator.ResetTrigger(Attack1);
            _myAnimator.SetTrigger(StopAttack1);
        }

        #region Animation Events

        private void Hit()
        {
            if (_target == null) return;

            //  BaseStats.GetStat calculates the additive damage from any IModifierProvider and deals the total damage
            if (_currentWeapon.value.HasProjectile())
            {   //  Ranged Attack
                _currentWeapon.value.LaunchProjectile(rightHandTransform, leftHandTransform, _target, gameObject, GetComponent<BaseStats>().GetStat(Stat.RANGE_DMG));
            }
            else
            {   //  Melee Attack
                _target.TakeDamage(gameObject, GetComponent<BaseStats>().GetStat(Stat.MELEE_DMG));
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
            _target = null;
        }   //  IAction

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (stat == Stat.MELEE_DMG || stat == Stat.RANGE_DMG)
            {
                yield return _currentWeapon.value.GetWeaponBaseDamage();
            }
        }   //  IModifierProvider

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.MELEE_DMG || stat == Stat.RANGE_DMG)
            {
                yield return _currentWeapon.value.GetPercentageBonus();
            }
        }   //  IModifierProvider

        public object CaptureState()
        {
            print("Trying to get LazyValue<Weapon> Name !");
            return _currentWeapon.value.name;
        }   //  ISaveable

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            WeaponConfig weapon = Resources.Load<WeaponConfig>(weaponName);
            EquipWeapon(weapon);
        }   //  ISaveable                
        #endregion
    }
}