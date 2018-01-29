// Allan Murillo : Unity RPG Core Test Project
using UnityEngine;


namespace RPG {

    public abstract class SpellBehaviour : MonoBehaviour {


        SpellConfig config;


        public abstract void Activate(SpellUseParams spell);
    }
}