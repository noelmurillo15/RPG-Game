// Allan Murillo : Unity RPG Core Test Project
using UnityEngine;


namespace RPG {

    [CreateAssetMenu(menuName = ("RPG/Spell/Buff"))]
    public class BuffSpellConfig : Spell {


        [Header("Buff Spell")]   //  Header for inspector setting grouping
        [SerializeField] BuffType buff;
        [SerializeField] float statChangeAmt = 1f;


        public override void AttachComponent(GameObject gameObjToAttachTo)
        {
            var behaviourComponent = gameObjToAttachTo.AddComponent<BuffSpellBehaviour>();
            behaviourComponent.SetConfig(this);
            behaviour = behaviourComponent;
        }

        public float GetStatChangeAmount() { return statChangeAmt; }

        public BuffType GetBuffType() { return buff; }
    }
}