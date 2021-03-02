/*
 * BuffSpellBehaviour -
 * Created By : Allan Murillo
 * Last Edited : 2/21/2020
 */

using ANM.Control;
using UnityEngine;

namespace RPG.SpellSystem
{
    public class BuffSpellBehaviour : SpellBehaviour
    {
        private void Start()
        {
            caster = GetComponent<PlayerController>();
        }

        public override void Activate(GameObject spellParams = null)
        {
            ApplyBuff(spellParams);
        }

        private void ApplyBuff(GameObject spellParams)
        {
            var buffSpellConfig = (config as BuffSpellConfig);

            if(spellParams != null)
            {   //  Buff target
                PlayParticleEffect(spellParams.transform);
            }
            else
            {
                //  Self Buff
                PlayParticleEffect(caster.transform);
                //if (buffSpellConfig != null) caster.EventCallPlayerHpIncrease(buffSpellConfig.GetStatChangeAmount());
            }
        }

        private void ApplyDebuff(GameObject spellParams)
        {
            var buffSpellConfig = (config as BuffSpellConfig);

            if(spellParams != null)
            {   //  Buff target
                PlayParticleEffect(spellParams.transform);
            }
            else
            {
                //  Self Buff
                PlayParticleEffect(caster.transform);
                //if (buffSpellConfig != null) caster.EventCallPlayerHpDeduction(buffSpellConfig.GetStatChangeAmount());
            }
        }
    }
}
