// Allan Murillo : Unity RPG Core Test Project
using UnityEngine;


namespace RPG {

    public class AoeSpellBehaviour : MonoBehaviour, ISpell {


        AoeSpellConfig config;


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
            float damageToDeal = spellParams.baseDamage + config.GetDamage();

            //  Static Sphere Cast for targets
            RaycastHit[] hits = Physics.SphereCastAll(
                transform.position,
                config.GetRadius(),
                Vector3.up,
                config.GetRadius()
            );

            foreach (RaycastHit hit in hits)
            {
                var damageable = hit.collider.gameObject.GetComponent<IDamageable>();

                if (damageable != null)
                {
                    damageable.TakeDamage(damageToDeal);
                }
            }            
        }
    }
}