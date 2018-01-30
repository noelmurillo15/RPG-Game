// Allan Murillo : Unity RPG Core Test Project
using UnityEngine;


namespace RPG {

    public enum BuffType
    {
        NONE,
        HP,
        MANA,
    }

    public abstract class SpellConfig : ScriptableObject {


        #region Properties
        [Header("Spell General Settings")]
        [SerializeField]
        float manaCost = 10f;

        //  References
        [SerializeField] AudioClip audio;
        [SerializeField] AnimationClip animation;
        [SerializeField] GameObject particlePrefab;

        protected SpellBehaviour behaviour;
        #endregion



        public abstract SpellBehaviour GetUniqueBehaviour(GameObject objAttached);

        public float GetManaCost() { return manaCost; }

        public AudioClip GetAudio() { return audio; }

        public AnimationClip GetAnimation() { return animation; }

        public GameObject GetParticles() { return particlePrefab; }

        public void AttachSpell(GameObject objAttached)
        {
            SpellBehaviour behaviourComponent = GetUniqueBehaviour(objAttached);
            behaviourComponent.SetConfig(this);
            behaviour = behaviourComponent;
        }

        public void Activate(GameObject target)
        {
            behaviour.Activate(target);
        }        
    }
}