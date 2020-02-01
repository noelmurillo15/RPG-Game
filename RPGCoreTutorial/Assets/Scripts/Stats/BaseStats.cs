using System;
using System.Linq;
using GameDevTV.Utils;
using UnityEngine;

namespace ANM.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [SerializeField] [Range(1, 30)] private int startingLevel = 1;
        [SerializeField] private CharacterClasses characterClass;
        [SerializeField] private Progression progression = null;
        [SerializeField] private GameObject levelUpEffect = null;
        [SerializeField] private bool shouldUseModifiers = false;

        //  Cached variables
        private Experience _experience;

        //  My Variables
        private LazyValue<int> _currentLvl;

        //  Events
        public event Action OnLevelUp;


        private void Awake()
        {
            _experience = GetComponent<Experience>();
            _currentLvl = new LazyValue<int>(CalcLevel);
        }   //  Used to cache references - no external functions should be called here

        private void Start()
        {
            _currentLvl.ForceInit();
        }

        private void OnEnable()
        {
            if (_experience != null)
            {
                _experience.OnExperienceGained += UpdateLevel;
            }
        }

        private void OnDisable()
        {
            if (_experience != null)
            {
                _experience.OnExperienceGained -= UpdateLevel;
            }
        }

        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat) / 100);
        }

        public int GetLevel()
        {
            return _currentLvl.value;
        }

        private int CalcLevel()
        {
            if (_experience == null) return startingLevel;

            float currentXP = _experience.GetExperiencePts();
            int penultimateLevel = progression.GetLevels(Stat.EXP_TO_LVL, characterClass);

            for (int level = 1; level <= penultimateLevel; level++)
            {
                float expToLvlUp = progression.GetStat(Stat.EXP_TO_LVL, characterClass, level);
                if (expToLvlUp > currentXP) { return level; }
            }

            return penultimateLevel + 1;
        }

        private void UpdateLevel()
        {
            var newLevel = CalcLevel();
            if (newLevel <= GetLevel()) return;
            _currentLvl.value = newLevel;
            LevelUpEffect();
            OnLevelUp();
        }

        private void LevelUpEffect()
        {
            Instantiate(levelUpEffect, transform);
        }

        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        private float GetAdditiveModifier(Stat stat)
        {
            return GetComponents<IModifierProvider>().SelectMany(provider => provider.GetAdditiveModifiers(stat)).Sum();
        }

        private float GetPercentageModifier(Stat stat)
        {
            if (!shouldUseModifiers) return 0;

            return GetComponents<IModifierProvider>().SelectMany(provider => provider.GetPercentageModifiers(stat)).Sum();
        }
    }
}