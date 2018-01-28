// Allan Murillo : Unity RPG Core Test Project
using UnityEngine;


namespace RPG {
    
    public abstract class SpellConfig : ScriptableObject {


        [Header("Spell General")]   //  Header for inspector setting grouping
        [SerializeField]
        float manaCost = 10f;
        [SerializeField] float dmgMultiplier = 2f;
        [SerializeField] AnimationClip animation;



        abstract public ISpell AddComponent(GameObject gameObjToAttachTo);
    }
}