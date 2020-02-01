using ANM.Control;
using UnityEngine;
using Random = UnityEngine.Random;


namespace ANM.Stats
{
    [SelectionBase]
    public class CharacterStats : MonoBehaviour
    {
        #region Variables
        [Header("Character Info")]
        [SerializeField] private CharacterTypes characterType;
        [SerializeField] private CharacterRanks characterRank;
        [SerializeField] private CharacterClasses characterClass;

        [Header("Experience")]
        [SerializeField] [Range(1, 30)] private int currentLevel;
        [SerializeField] private float currentExp;
        [SerializeField] private float expToNextLvl;
        [SerializeField] private float expMultiplier;

        [Header("Attributes")]
        [SerializeField] private int strength;
        [SerializeField] private int wisdom;
        [SerializeField] private int endurance;
        [SerializeField] private int luck;

        [Header("Stats")]
        [SerializeField] private int physicalAttack = 0;
        [SerializeField] private int magicalAttack = 0;
        [SerializeField] private int damageResist = 0;
        [SerializeField] private int criticalRate = 0;
        [SerializeField] private int criticalDamage = 0;
        #endregion


        private void Start()
        {
            Initialize();
            LevelUp();
        }

        #region Private Methods

        private void Initialize()
        {
            if (GetComponent<AIController>() != null)
            {
                characterType = CharacterTypes.ENEMY;
            }
            else if (GetComponent<PlayerController>() != null)
            {
                characterType = CharacterTypes.PLAYER;
            }

            physicalAttack = Random.Range(1, 10);
            magicalAttack = Random.Range(1, 10);
            damageResist = Random.Range(0, 5);
            criticalRate = Random.Range(0, 5);
            criticalDamage = Random.Range(10, 25);
        }

        private void LevelUp()
        {
            currentLevel++;
            currentLevel = Mathf.Clamp(currentLevel, 0, 30);

            if (characterType == CharacterTypes.ENEMY)
            {
                AssignAttribute();
                physicalAttack += 1;
                magicalAttack += 1;
                damageResist += 1;
            }
            RankUp();
        }

        private void RankUp()
        {
            switch (currentLevel)
            {
                case 6:
                    characterRank = CharacterRanks.COMMON;
                    break;
                case 11:
                    characterRank = CharacterRanks.ELITE;
                    break;
                case 16:
                    characterRank = CharacterRanks.LEGENDARY;
                    break;
                case 21:
                    characterRank = CharacterRanks.UBER;
                    break;
                case 26:
                    characterRank = CharacterRanks.BOSS;
                    break;
                case 30:
                    Debug.Log(characterType + " has achieved God Rank!");
                    break;
            }
        }

        private void AssignAttribute()
        {
            int ran = Random.Range(0, 4);
            switch (ran)
            {
                case 0:
                    IncreaseStrength();
                    break;
                case 1:
                    IncreaseWisdom();
                    break;
                case 2:
                    IncreaseEndurance();
                    break;
                case 3:
                    IncreaseLuck();
                    break;
            }
        }

        private void IncreaseStrength()
        {
            strength++;
            physicalAttack += 3;
            if (characterClass != CharacterClasses.NONE || strength <= 10) return;
            physicalAttack += 20;
            characterClass = CharacterClasses.ASSASSIN;
        }

        private void IncreaseWisdom()
        {
            wisdom++;
            magicalAttack += 3;
            if (characterClass != CharacterClasses.NONE || wisdom <= 10) return;
            magicalAttack += 20;
            characterClass = CharacterClasses.WARLOCK;
        }

        private void IncreaseEndurance()
        {
            endurance++;
            damageResist += 2;
            if (characterClass != CharacterClasses.NONE || endurance <= 10) return;
            damageResist += 15;
            characterClass = CharacterClasses.TANK;
        }

        private void IncreaseLuck()
        {
            luck++;
            criticalRate += 1;
            criticalDamage += 5;
            expMultiplier += 0.1f;
        }
        #endregion

        #region Accessors
        public CharacterTypes GetCharacterType() { return characterType; }
        public CharacterRanks GetCharacterRanks() { return characterRank; }
        public CharacterClasses GetCharacterClass() { return characterClass; }

        public int GetPhysicalAttack() { return physicalAttack; }
        public int GetMagicalAttack() { return magicalAttack; }

        //  TODO : Apply these
        public int GetDamageResist() { return damageResist; }
        public int GetCriticalRate() { return criticalRate; }
        public int GetCriticalDamage() { return criticalDamage; }
        #endregion
    }
}