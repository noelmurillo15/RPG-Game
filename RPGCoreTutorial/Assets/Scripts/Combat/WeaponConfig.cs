using UnityEngine;
using RPG.Attributes;


namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class WeaponConfig : ScriptableObject
    {
        [SerializeField] GameObject equippedPrefab = null;
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] float range = 2f;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] float weaponBaseDamage = 20f;
        [SerializeField] float weaponPercentageBonus = 0f;
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

            //  Default Animator Override Controller  
            var overrideController = _animator.runtimeAnimatorController as AnimatorOverrideController;
            if (animatorOverride != null)
            {
                _animator.runtimeAnimatorController = animatorOverride;
            }
            else if (overrideController != null)
            {
                _animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
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

        public void LaunchProjectile(Transform _rightHand, Transform _leftHand, Health _target, GameObject _instigator, float _totalDmg)
        {
            Projectile projectileInstance = Instantiate(projectile, _leftHand.position, Quaternion.identity);
            projectileInstance.SetTarget(_target, _instigator, _totalDmg);
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public float GetWeaponRange()
        {
            return range;
        }

        public float GetPercentageBonus()
        {
            return weaponPercentageBonus;
        }

        public float GetWeaponBaseDamage()
        {
            return weaponBaseDamage;
        }

        public float GetWeaponFireRate()
        {
            return timeBetweenAttacks;
        }
    }
}