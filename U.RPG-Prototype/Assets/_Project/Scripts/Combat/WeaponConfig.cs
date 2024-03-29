﻿/*
 * WeaponConfig - Contains data needed to create runtime weapons
 * Created by : Allan N. Murillo
 * Last Edited : 2/25/2020
 */

using UnityEngine;
using ANM.Attributes;

namespace ANM.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class WeaponConfig : ScriptableObject
    {
        [SerializeField] private GameObject equippedPrefab = null;
        [SerializeField] private AnimatorOverrideController animatorOverride = null;
        [SerializeField] private float range = 2f;
        [SerializeField] private float timeBetweenAttacks = 1f;
        [SerializeField] private float weaponBaseDamage = 20f;
        [SerializeField] private float weaponPercentageBonus = 0f;
        [SerializeField] private bool isRightHanded = true;
        [SerializeField] private Projectile projectile = null;

        private const string WeaponName = "MyWeapon";


        public void SpawnWeapon(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);

            if (equippedPrefab != null)
            {
                if (isRightHanded)
                {
                    var weapon = Instantiate(equippedPrefab, rightHand);
                    weapon.name = WeaponName;
                }
                else
                {
                    var weapon = Instantiate(equippedPrefab, leftHand);
                    weapon.name = WeaponName;
                }
            }

            //  Default Animator Override Controller  
            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (animatorOverride != null)
                animator.runtimeAnimatorController = animatorOverride;
            else if (overrideController != null)
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
        }

        private static void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            var oldWeapon = rightHand.Find(WeaponName);

            if (oldWeapon == null)
                oldWeapon = leftHand.Find(WeaponName);

            if (oldWeapon == null) return;

            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float totalDmg)
        {
            Projectile projectileInstance = Instantiate(projectile, leftHand.position, Quaternion.identity);
            projectileInstance.SetTarget(target, instigator, totalDmg);
        }

        public bool HasProjectile() { return projectile != null; }

        public float GetWeaponRange() { return range; }

        public float GetPercentageBonus() { return weaponPercentageBonus; }

        public float GetWeaponBaseDamage() { return weaponBaseDamage; }

        public float GetWeaponFireRate() { return timeBetweenAttacks; }
    }
}