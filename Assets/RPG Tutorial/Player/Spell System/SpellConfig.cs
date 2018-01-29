// Allan Murillo : Unity RPG Core Test Project
using UnityEngine;


namespace RPG {

    public enum BuffType {
        NONE,
        HP,
        MANA,
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

    public abstract class SpellConfig : ScriptableObject {


        [Header("Spell General Settings")]   //  Header for inspector setting grouping
        [SerializeField] float manaCost = 10f;

        [SerializeField] AudioClip audio;
        [SerializeField] AnimationClip animation;
        [SerializeField] GameObject particlePrefab;

        protected SpellBehaviour behaviour;



        public abstract SpellBehaviour GetUniqueBehaviour(GameObject objAttached);

        public void AttachSpell(GameObject objAttached)
        {
            SpellBehaviour behaviourComponent = GetUniqueBehaviour(objAttached);
            behaviourComponent.SetConfig(this);
            behaviour = behaviourComponent;
        }        

        public void Activate(SpellUseParams mySpell)
        {
            behaviour.Activate(mySpell);
        }
      
        public float GetManaCost() { return manaCost; }
        public AudioClip GetAudio() { return audio; }
        public AnimationClip GetAnimation() { return animation; }
        public GameObject GetParticles() { return particlePrefab; }
    }
}