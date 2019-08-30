using RPG.Core;
using UnityEngine;


namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] GameObject equippedPrefab = null;
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] float range = 2f;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] float damage = 20f;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;


        public void SpawnWeapon(Transform _rightHand, Transform _leftHand, Animator _animator)
        {
            if (equippedPrefab != null)
            {
                if (isRightHanded)
                {
                    Instantiate(equippedPrefab, _rightHand);
                }
                else
                {
                    Instantiate(equippedPrefab, _leftHand);
                }

            }
            if (animatorOverride != null)
            {
                _animator.runtimeAnimatorController = animatorOverride;
            }
        }

        public void LaunchProjectile(Transform _rightHand, Transform _leftHand, Health _target)
        {
            Projectile projectileInstance = Instantiate(projectile, _leftHand.position, Quaternion.identity);
            projectileInstance.SetTarget(_target, damage);
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public float GetWeaponRange()
        {
            return range;
        }

        public float GetWeaponDamage()
        {
            return damage;
        }

        public float GetTimeBetweenAttacks()
        {
            return timeBetweenAttacks;
        }
    }
}