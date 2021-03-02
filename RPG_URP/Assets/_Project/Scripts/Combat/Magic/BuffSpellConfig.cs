/*
 * BuffSpellConfig -
 * Created by : Allan N. Murillo
 * Last Edited : 4/7/2020
 */

using UnityEngine;

namespace RPG.SpellSystem
{
    [CreateAssetMenu(menuName = ("RPG/Spells/Buff"))]
    public class BuffSpellConfig : SpellConfig
    {
        [Header("Buff Settings")]
        [SerializeField]
        private BuffType buff = BuffType.HP;
        [SerializeField] private float statChangeAmt = 1f;


        protected override SpellBehaviour GetUniqueBehaviour(GameObject objAttached)
        {
            return objAttached.AddComponent<BuffSpellBehaviour>();
        }

        public BuffType GetBuffType() { return buff; }
        public float GetStatChangeAmount() { return statChangeAmt; }
    }
}