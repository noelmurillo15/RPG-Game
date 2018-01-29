// Allan Murillo : Unity RPG Core Test Project
using UnityEngine;


namespace RPG {

    public interface ISpell {

        void Activate(SpellUseParams spellParams);
    }

    public struct SpellUseParams {
        public IDamageable target;
        public float baseDamage;

        public SpellUseParams(IDamageable target, float baseDamage)
        {
            this.target = target;
            this.baseDamage = baseDamage;
        }
    }

    public abstract class Spell : ScriptableObject {


        [Header("Spell General")]   //  Header for inspector setting grouping
        [SerializeField] float manaCost = 10f;
        [SerializeField] float dmgMultiplier = 2f;
        [SerializeField] AnimationClip animation;

        protected ISpell behaviour;



        abstract public void AttachComponent(GameObject gameObjToAttachTo);

        public void Activate(SpellUseParams spellParams)
        {
            behaviour.Activate(spellParams);
        }

        public float GetManaCost() { return manaCost; }
    }
}