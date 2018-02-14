﻿// Allan Murillo : Unity RPG Core Test Project
using UnityEngine;


namespace RPG {

    public class AoeSpellBehaviour : SpellBehaviour {


        [SerializeField] PlayerMaster caster;



        private void Start()
        {
            caster = GetComponent<PlayerMaster>();
        }

        public override void Activate(GameObject spellParams)
        {
            DealRadialDamage(caster.MagicalAttack);
        }

        #region Area Attack
        private void DealRadialDamage(float baseDmg)
        {
            var aoeSpellConfig = (config as AoeSpellConfig);

            //  Static Sphere Cast for targets
            RaycastHit[] hits = Physics.SphereCastAll(
                transform.position,
                aoeSpellConfig.GetRadius() + caster.MagicRange,
                Vector3.up,
                aoeSpellConfig.GetRadius() + caster.MagicRange
            );

            float damageToDeal = baseDmg + aoeSpellConfig.GetDamage();

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    continue;
                }

                var damageable = hit.collider.gameObject.GetComponent<HealthSystem>();
                if (damageable != null)
                {
                    damageable.TakeDamage(damageToDeal);
                }
            }
            PlayParticleEffect();
        }
        #endregion
    }
}