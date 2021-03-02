/*
 * AoeSpellConfig -
 * Created by : Allan N. Murillo
 * Last Edited : 4/7/2020
 */

using UnityEngine;

namespace RPG.SpellSystem
{
    [CreateAssetMenu(menuName = ("RPG/Spells/Area of Effect"))]
    public class AoeSpellConfig : SpellConfig
    {
        [Header("Area of Effect Settings")]
        [SerializeField]
        private float radius = 5f;
        [SerializeField] private float damage = 15f;
        [SerializeField] private float groundOffset = 0f;


        protected override SpellBehaviour GetUniqueBehaviour(GameObject objAttached)
        {
            return objAttached.AddComponent<AoeSpellBehaviour>();
        }

        public float GetDamage() { return damage; }
        public float GetRadius() { return radius; }
        public float GetOffset() { return groundOffset; }
    }
}