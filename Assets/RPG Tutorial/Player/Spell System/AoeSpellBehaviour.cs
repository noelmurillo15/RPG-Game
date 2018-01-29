// Allan Murillo : Unity RPG Core Test Project
using System;
using UnityEngine;


namespace RPG {

    public class AoeSpellBehaviour : MonoBehaviour, ISpell {


        AoeSpellConfig config;
        ParticleSystem myParticleSystem;


        void Start()
        {
            print("Aoe Spell behaviour Attached");
        }

        public void SetConfig(AoeSpellConfig configToAttach)
        {
            this.config = configToAttach;
        }

        public void Activate(SpellUseParams spellParams)
        {
            PlayParticleEffect();
            DealRadialDamage(spellParams.baseDamage);
        }

        private void PlayParticleEffect()
        {
            //  Instantiate particle system prefab attached to player
            var prefab = Instantiate(config.GetParticlePrefab(), transform.position, Quaternion.identity);
            //  Get the particle system component
            myParticleSystem = prefab.GetComponent<ParticleSystem>();
            //  Play particle system
            myParticleSystem.Play();
            //  Destroy particle system
            Destroy(prefab, myParticleSystem.main.duration);
        }

        private void DealRadialDamage(float baseDmg)
        {
            //  Static Sphere Cast for targets
            RaycastHit[] hits = Physics.SphereCastAll(
                transform.position,
                config.GetRadius(),
                Vector3.up,
                config.GetRadius()
            );

            float damageToDeal = baseDmg + config.GetDamage();

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    continue;
                }

                var damageable = hit.collider.gameObject.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(damageToDeal);
                }
            }
        }
    }
}