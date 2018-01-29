// Allan Murillo : Unity RPG Core Test Project
using UnityEngine;


namespace RPG {

    [CreateAssetMenu(menuName = ("RPG/Spell/Buff"))]
    public class BuffSpellConfig : SpellConfig {


        [Header("Buff Settings")]   //  Header for inspector setting grouping
        [SerializeField] BuffType buff;
        [SerializeField] float statChangeAmt = 1f;

    

        public BuffType GetBuffType() { return buff; }

        public float GetStatChangeAmount() { return statChangeAmt; }

        public override SpellBehaviour GetUniqueBehaviour(GameObject objAttached)
        {
            return objAttached.AddComponent<BuffSpellBehaviour>();
        }
    }
}