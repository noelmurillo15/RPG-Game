using UnityEngine;


namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [SerializeField] [Range(1, 30)] int startingLevel = 1;
        [SerializeField] CharacterClasses characterClass;
        [SerializeField] Progression progression = null;

        public float GetHealth(){
            return progression.GetHealth(characterClass, startingLevel);
        }
    }
}