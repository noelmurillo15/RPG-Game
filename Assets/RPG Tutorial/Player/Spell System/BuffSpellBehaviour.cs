// Allan Murillo : Unity RPG Core Test Project
using UnityEngine;


namespace RPG {

    public class BuffSpellBehaviour : MonoBehaviour, ISpell {


        BuffSpellConfig config;

        

        void Start()
        {
            //print("Buff Spell behaviour Attached");            
        }

        public void SetConfig(BuffSpellConfig configToAttach)
        {
            this.config = configToAttach;
        }

        public void Activate(SpellUseParams spellParams)
        {
            spellParams.target.StatChange(config.GetBuffType(), config.GetStatChangeAmount());
        }
    }
}