// Allan Murillo : Unity RPG Core Test Project
using UnityEngine;


namespace RPG {

    [CreateAssetMenu(menuName = ("RPG/Spell/Projectile"))]
    public class ProjectileConfig : SpellConfig {


        [Header("Spell Projectile")]   //  Header for inspector setting grouping
        [SerializeField] float velocity;


        public override ISpell AddComponent(GameObject gameObjToAttachTo) {
            var behaviourComponent = gameObjToAttachTo.AddComponent<ProjectileSpellBehaviour>();
            behaviourComponent.SetConfig(this);
            return behaviourComponent;
        }
    }
}