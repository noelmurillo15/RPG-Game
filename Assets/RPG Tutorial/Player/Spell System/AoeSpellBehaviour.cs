// Allan Murillo : Unity RPG Core Test Project
using UnityEngine;


namespace RPG {

    public class AoeSpellBehaviour : SpellBehaviour {


        public override void Activate(SpellUseParams spellParams)
        {
            DealRadialDamage(spellParams.baseDamage);
        }
     
        private void DealRadialDamage(float baseDmg)
        {
            var aoeSpellConfig = (config as AoeSpellConfig);

            //  Static Sphere Cast for targets
            RaycastHit[] hits = Physics.SphereCastAll(
                transform.position,
                aoeSpellConfig.GetRadius(),
                Vector3.up,
                aoeSpellConfig.GetRadius()
            );

            float damageToDeal = baseDmg + aoeSpellConfig.GetDamage();

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    continue;
                }

                var damageable = hit.collider.gameObject.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.AdjustHealth(damageToDeal * -1f);
                }
            }
            PlayParticleEffect();
        }
    }
}