// Allan Murillo : Unity RPG Core Test Project
using UnityEngine;


namespace RPG {

    [CreateAssetMenu(menuName = ("RPG/Spell/Area of Effect"))]
    public class AoeSpellConfig : SpellConfig
    {


        [Header("Area of Effect Settings")]   //  Header for inspector setting grouping
        [SerializeField] float radius = 5f;
        [SerializeField] float damage = 15f;


        public override void AttachComponent(GameObject gameObjToAttachTo)
        {
            var behaviourComponent = gameObjToAttachTo.AddComponent<AoeSpellBehaviour>();
            behaviourComponent.SetConfig(this);
            behaviour = behaviourComponent;
        }

        public float GetDamage() { return damage; }

        public float GetRadius() { return radius; }
    }
}