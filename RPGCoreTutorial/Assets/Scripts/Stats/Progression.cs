using UnityEngine;


namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClass = null;


        public float GetHealth(CharacterClasses _class, int level){
            foreach (ProgressionCharacterClass proClass in characterClass)
            {
                if(proClass.characterClass == _class){
                    return proClass.health[level - 1];
                }
            }

            Debug.LogError("Returning Default Health");
            return 0;
        }

        [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClasses characterClass;
            public float[] health;
        }
    }
}