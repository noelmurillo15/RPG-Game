using UnityEngine;
using System.Collections.Generic;


namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClass = null;

        Dictionary<CharacterClasses, Dictionary<Stat, float[]>> lookUpTable = null;


        public float GetStat(Stat _stat, CharacterClasses _class, int _level)
        {
            BuildLookUp();

            float[] levels = lookUpTable[_class][_stat];

            if (levels.Length < _level) { return 0; }

            return levels[_level - 1];
        }   //  Performant Dictionary Lookup

        public int GetLevels(Stat _stat, CharacterClasses _class)
        {
            BuildLookUp();

            float[] levels = lookUpTable[_class][_stat];

            return levels.Length;
        }

        void BuildLookUp()
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
        class ProgressionCharacterClass
        {
            public CharacterClasses characterClass;
            public ProgressionStat[] stats;
        }

        [System.Serializable]
        class ProgressionStat
        {
            public Stat stat;
            public float[] levels;
        }
    }
}