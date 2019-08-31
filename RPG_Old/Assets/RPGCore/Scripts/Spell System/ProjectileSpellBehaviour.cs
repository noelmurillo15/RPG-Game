// Allan Murillo : Unity RPG Core Test Project
using UnityEngine;


namespace RPG.Stats
{

    public class ProjectileSpellBehaviour : SpellBehaviour {


        [SerializeField] CharacterStats caster;



        private void Start()
        {
            caster = GetComponent<CharacterStats>();
        }

        public override void Activate(GameObject spellParams)
        {
            FireProjectile(spellParams);
        }

        private void FireProjectile(GameObject spellParams)
        {
            var projectileSpellConfig = (config as ProjectileSpellConfig);

            float damageToDeal = caster.MagicalAttack + projectileSpellConfig.GetDamage();
            spellParams.GetComponent<Character>().CallEventCharacterTakeDamage(damageToDeal);

            PlayParticleEffect();
        }
    }
}