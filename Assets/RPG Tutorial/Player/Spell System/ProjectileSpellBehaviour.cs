// Allan Murillo : Unity RPG Core Test Project
using UnityEngine;


namespace RPG {

    public class ProjectileSpellBehaviour : SpellBehaviour {


        public override void Activate(SpellUseParams spellParams)
        {
            var projectileSpellConfig = (config as ProjectileSpellConfig);

            float damageToDeal = spellParams.baseDamage + projectileSpellConfig.GetDamage();
            spellParams.target.AdjustHealth(damageToDeal * -1f);

            PlayParticleEffect();
        }
    }
}