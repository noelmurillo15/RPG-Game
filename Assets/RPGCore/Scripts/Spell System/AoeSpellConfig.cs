// Allan Murillo : Unity RPG Core Test Project
using UnityEngine;


namespace RPG {

    [CreateAssetMenu(menuName = ("RPG/Spell/Area of Effect"))]
    public class AoeSpellConfig : SpellConfig {


        [Header("Area of Effect Settings")]   
        [SerializeField] float radius = 5f;
        [SerializeField] float damage = 15f;

      

        public float GetDamage() { return damage; }

        public float GetRadius() { return radius; }

        public override SpellBehaviour GetUniqueBehaviour(GameObject objAttached)
        {
            return objAttached.AddComponent<AoeSpellBehaviour>();
        }
    }
}