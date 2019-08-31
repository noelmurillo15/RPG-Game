// Allan Murillo : Unity RPG Core Test Project
using UnityEngine;


namespace RPG.Stats
{

    public class BuffSpellBehaviour : SpellBehaviour {


        public override void Activate(GameObject spellParams)
        {
            ApplyBuff(spellParams);
        }

        private void ApplyBuff(GameObject spellParams)
        {
            var buffSpellConfig = (config as BuffSpellConfig);

            spellParams.GetComponent<Character>().CallEventCharacterHeal(buffSpellConfig.GetStatChangeAmount());

            PlayParticleEffect();
        }
    }
}