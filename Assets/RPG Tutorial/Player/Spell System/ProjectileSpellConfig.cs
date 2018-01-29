// Allan Murillo : Unity RPG Core Test Project
using UnityEngine;


namespace RPG {

    [CreateAssetMenu(menuName = ("RPG/Spell/Projectile"))]
    public class ProjectileSpellConfig : Spell {


        [Header("Projectile Spell")]   //  Header for inspector setting grouping
        [SerializeField] float damage;


        public override void AttachComponent(GameObject gameObjToAttachTo)
        {
            var behaviourComponent = gameObjToAttachTo.AddComponent<ProjectileSpellBehaviour>();
            behaviourComponent.SetConfig(this);
            behaviour = behaviourComponent;
        }

        public float GetDamage() { return damage; }
    }
}