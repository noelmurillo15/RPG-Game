// Allan Murillo : Unity RPG Core Test Project
using UnityEngine;


namespace RPG.Stats
{

    [CreateAssetMenu(menuName = ("RPG/Spell/Buff"))]
    public class BuffSpellConfig : SpellConfig {


        [Header("Buff Settings")]   
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