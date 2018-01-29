// Allan Murillo : Unity RPG Core Test Project
using UnityEngine;


namespace RPG {

    public class BuffSpellBehaviour : SpellBehaviour {


        public override void Activate(SpellUseParams spellParams)
        {
            var buffSpellConfig = (config as BuffSpellConfig);           

            spellParams.target.StatChange(
                buffSpellConfig.GetBuffType(),
                buffSpellConfig.GetStatChangeAmount());

            PlayParticleEffect();
        }
    }
}