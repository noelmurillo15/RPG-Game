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
        [SerializeField] float baseDamage = 20f;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;

        const string weaponName = "MyWeapon";


        public void SpawnWeapon(Transform _rightHand, Transform _leftHand, Animator _animator)
        {
            DestroyOldWeapon(_rightHand, _leftHand);

            if (equippedPrefab != null)
            {
                if (isRightHanded)
                {
                    GameObject weapon = Instantiate(equippedPrefab, _rightHand);
                    weapon.name = weaponName;
                }
                else
                {
                    GameObject weapon = Instantiate(equippedPrefab, _leftHand);
                    weapon.name = weaponName;
                }

            }
            if (animatorOverride != null)
            {
                _animator.runtimeAnimatorController = animatorOverride;
            }
        }

        void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(weaponName);

            if (oldWeapon == null)
                oldWeapon = leftHand.Find(weaponName);

            if (oldWeapon == null) return;

            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }

        public void LaunchProjectile(Transform _rightHand, Transform _leftHand, Health _target, float _damage)
        {
            Projectile projectileInstance = Instantiate(projectile, _leftHand.position, Quaternion.identity);
            projectileInstance.SetTarget(_target, baseDamage + _damage);
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
            return baseDamage;
        }

        public float GetTimeBetweenAttacks()
        {
            return timeBetweenAttacks;
        }
    }
}