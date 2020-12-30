/*
 * Progression - 
 * Created by : Allan N. Murillo
 * Last Edited : 2/25/2020
 */

using UnityEngine;
using System.Collections.Generic;

namespace ANM.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] private ProgressionCharacterClass[] characterClass = null;

        private Dictionary<CharacterClasses, Dictionary<Stat, float[]>> _lookUpTable;


        public float GetStat(Stat stat, CharacterClasses @class, int level)
        {
            BuildLookUp();

            float[] levels = _lookUpTable[@class][stat];

            if (levels.Length < level) { return 0; }

            return levels[level - 1];
        }   //  Performant Dictionary Lookup

        public int GetLevels(Stat stat, CharacterClasses @class)
        {
            BuildLookUp();

            float[] levels = _lookUpTable[@class][stat];

            return levels.Length;
        }

        private void BuildLookUp()
        {
            if (_lookUpTable != null) return;

            _lookUpTable = new Dictionary<CharacterClasses, Dictionary<Stat, float[]>>();

            foreach (var progressionClass in characterClass)
            {
                var statLookUpTable = new Dictionary<Stat, float[]>();

                foreach (var progressionStat in progressionClass.stats)
                {
                    statLookUpTable[progressionStat.stat] = progressionStat.levels;
                }

                _lookUpTable[progressionClass.characterClass] = statLookUpTable;
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