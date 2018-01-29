// Allan Murillo : Unity RPG Core Test Project
using UnityEngine;


namespace RPG {

    public class ProjectileSpellBehaviour : SpellBehaviour
    {


        ProjectileSpellConfig config;


        void Start()
        {
            //print("Projectile Spell behaviour Attached");
        }

        public void SetConfig(ProjectileSpellConfig configToAttach)
        {
            this.config = configToAttach;
        }

        public override void Activate(SpellUseParams spellParams)
        {
            float damageToDeal = spellParams.baseDamage + config.GetDamage();
            spellParams.target.AdjustHealth(damageToDeal * -1f);
        }
    }
}