// Allan Murillo : Unity RPG Core Test Project
using UnityEngine;


namespace RPG {

    public class AoeSpellBehaviour : SpellBehaviour {


        AoeSpellConfig config;


        void Start()
        {
            //print("Aoe Spell behaviour Attached");
        }

        public void SetConfig(AoeSpellConfig configToAttach)
        {
            this.config = configToAttach;
        }

        public override void Activate(SpellUseParams spellParams)
        {
            DealRadialDamage(spellParams.baseDamage);
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
                    damageable.AdjustHealth(damageToDeal * -1f);
                }
            }
        }
    }
}