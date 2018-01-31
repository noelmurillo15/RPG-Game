// Allan Murillo : Unity RPG Core Test Project
using UnityEngine;


namespace RPG {

    public class ProjectileSpellBehaviour : SpellBehaviour {


        [SerializeField] Player caster;



        private void Start()
        {
            caster = GetComponent<Player>();
        }

        public override void Activate(GameObject spellParams)
        {
            FireProjectile(spellParams);
        }

        private void FireProjectile(GameObject spellParams)
        {
            var projectileSpellConfig = (config as ProjectileSpellConfig);

            float damageToDeal = caster.BaseDamage + projectileSpellConfig.GetDamage();
            spellParams.GetComponent<HealthSystem>().TakeDamage(damageToDeal);

            PlayParticleEffect();
        }
    }
}