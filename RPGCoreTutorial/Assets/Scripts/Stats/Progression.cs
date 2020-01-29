using UnityEngine;
using System.Collections.Generic;


namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] private ProgressionCharacterClass[] characterClass = null;

        private Dictionary<CharacterClasses, Dictionary<Stat, float[]>> lookUpTable = null;


        public float GetStat(Stat stat, CharacterClasses @class, int level)
        {
            BuildLookUp();

            float[] levels = lookUpTable[@class][stat];

            if (levels.Length < level) { return 0; }

            return levels[level - 1];
        }   //  Performant Dictionary Lookup

        public int GetLevels(Stat stat, CharacterClasses @class)
        {
            BuildLookUp();

            float[] levels = lookUpTable[@class][stat];

            return levels.Length;
        }

        private void BuildLookUp()
        {
            if (lookUpTable != null) return;

            lookUpTable = new Dictionary<CharacterClasses, Dictionary<Stat, float[]>>();

            foreach (ProgressionCharacterClass progressionClass in characterClass)
            {
                var statLookUpTable = new Dictionary<Stat, float[]>();

                foreach (ProgressionStat progressionStat in progressionClass.stats)
                {
                    statLookUpTable[progressionStat.stat] = progressionStat.levels;
                }

                lookUpTable[progressionClass.characterClass] = statLookUpTable;
            }
        }

        [System.Serializable]
        private class ProgressionCharacterClass
        {
            public CharacterClasses characterClass;
            public ProgressionStat[] stats;
        }

        [System.Serializable]
        private class ProgressionStat
        {
            public Stat stat;
            public float[] levels;
        }
    }
}