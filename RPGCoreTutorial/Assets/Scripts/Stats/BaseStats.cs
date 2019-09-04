using System;
using UnityEngine;


namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [SerializeField] [Range(1, 30)] int startingLevel = 1;
        [SerializeField] CharacterClasses characterClass;
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject levelUpEffect = null;
        [SerializeField] bool shouldUseModifiers = false;

        public event Action onLevelUp;

        int currentLvl = 0;


        void Start()
        {
            currentLvl = CalcLevel();
            Experience exp = GetComponent<Experience>();
            if (exp != null)
            {
                exp.onExperienceGained += UpdateLevel;
            }
        }

        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat) / 100);
        }

        float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        public int CalcLevel()
        {
            Experience exp = GetComponent<Experience>();
            if (exp == null) return startingLevel;

            float currentXP = exp.GetExperiencePts();
            int penultimateLevel = progression.GetLevels(Stat.EXP_TO_LVL, characterClass);

            for (int level = 1; level <= penultimateLevel; level++)
            {
                float expToLvlUp = progression.GetStat(Stat.EXP_TO_LVL, characterClass, level);
                if (expToLvlUp > currentXP) { return level; }
            }

            return penultimateLevel + 1;
        }

        public int GetLevel()
        {
            if (currentLvl < 1)
            {
                currentLvl = CalcLevel();
            }
            return currentLvl;
        }

        void UpdateLevel()
        {
            int newLevel = CalcLevel();
            if (newLevel > currentLvl)
            {
                currentLvl = newLevel;
                LevelUp();
                onLevelUp();
            }
        }

        void LevelUp()
        {
            Instantiate(levelUpEffect, transform);
        }

        float GetAdditiveModifier(Stat stat)
        {
            float total = 0f;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetAdditiveModifiers(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }

        float GetPercentageModifier(Stat stat)
        {
            if (!shouldUseModifiers) return 0;

            float total = 0f;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetPercentageModifiers(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }
    }
}