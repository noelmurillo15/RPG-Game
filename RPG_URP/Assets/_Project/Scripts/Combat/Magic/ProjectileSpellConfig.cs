/*
 * ProjectileSpellConfig -
 * Created by : Allan N. Murillo
 * Last Edited : 4/7/2020
 */

using UnityEngine;

namespace RPG.SpellSystem
{
    [CreateAssetMenu(menuName = ("RPG/Spells/Projectile"))]
    public class ProjectileSpellConfig : SpellConfig
    {
        [Header("Projectile Settings")]
        [SerializeField]
        private float damage = 0;


        protected override SpellBehaviour GetUniqueBehaviour(GameObject objAttached)
        {
            return objAttached.AddComponent<ProjectileSpellBehaviour>();
        }

        public float GetDamage() { return damage; }

        public void SetDamage(float dmg) { damage = dmg; }
    }
}