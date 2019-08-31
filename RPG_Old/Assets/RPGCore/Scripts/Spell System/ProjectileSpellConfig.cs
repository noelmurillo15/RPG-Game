// Allan Murillo : Unity RPG Core Test Project
using UnityEngine;


namespace RPG.Stats
{

    [CreateAssetMenu(menuName = ("RPG/Spell/Projectile"))]
    public class ProjectileSpellConfig : SpellConfig {


        [Header("Projectile Settings")]  
        [SerializeField] float damage;



        public float GetDamage() { return damage; }

        public override SpellBehaviour GetUniqueBehaviour(GameObject objAttached) {
            return objAttached.AddComponent<ProjectileSpellBehaviour>();
        }
    }
}