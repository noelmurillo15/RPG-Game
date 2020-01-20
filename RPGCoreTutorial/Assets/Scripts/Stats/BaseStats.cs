using System;
using System.Linq;
using UnityEngine;
using GameDevTV.Utils;


namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [SerializeField] [Range(1, 30)] int startingLevel = 1;
        [SerializeField] CharacterClasses characterClass;
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject levelUpEffect = null;
        [SerializeField] bool shouldUseModifiers = false;

        //  Cached variables
        Experience experience;

        //  My Variables
        LazyValue<int> currentLvl;

        //  Events
        public event Action onLevelUp;


        private void Awake()
        {
            experience = GetComponent<Experience>();
            currentLvl = new LazyValue<int>(CalcLevel);
        }   //  Used to cache references - no external functions should be called here

        private void Start()
        {
            currentLvl.ForceInit();
        }

        private void OnEnable()
        {
            if (experience != null)
            {
                experience.onExperienceGained += UpdateLevel;
            }
        }

        private void OnDisable()
        {
            if (experience != null)
            {
                experience.onExperienceGained -= UpdateLevel;
            }
        }

        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat) / 100);
        }

        public int GetLevel()
        {
            return currentLvl.value;
        }

        private int CalcLevel()
        {
            if (experience == null) return startingLevel;

            float currentXP = experience.GetExperiencePts();
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
            currentLvl.value = newLevel;
            LevelUpEffect();
            onLevelUp();
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