// Allan Murillo : Unity RPG Core Test Project
using UnityEngine;


namespace RPG {

    public class ProjectileSpellBehaviour : MonoBehaviour, ISpell {


        SpellConfig config;


        void Start()
        {
            print("Projectile Spell behaviour Attached");
        }

        public void SetConfig(SpellConfig configToAttach)
        {
            this.config = configToAttach;
        }

        public void Activate()
        {

        }
    }
}