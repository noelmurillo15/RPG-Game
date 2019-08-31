using UnityEngine;
using RPG.Control;
using Random = UnityEngine.Random;


namespace RPG.Stats
{
    [SelectionBase]
    public class CharacterStats : MonoBehaviour
    {
        #region Variables
        [Header("Character Info")]
        [SerializeField] CharacterTypes characterType;
        [SerializeField] CharacterRanks characterRank;
        [SerializeField] CharacterClasses characterClass;

        [Header("Experience")]
        [SerializeField] int currentLevel;
        [SerializeField] float currentExp;
        [SerializeField] float expToNextLvl;
        [SerializeField] float expMultiplier;

        [Header("Attributes")]
        [SerializeField] int strength;
        [SerializeField] int wisdom;
        [SerializeField] int endurance;
        [SerializeField] int luck;

        [Header("Stats")]
        [SerializeField] int physicalAttack = 0;
        [SerializeField] int magicalAttack = 0;
        [SerializeField] int damageResist = 0;
        [SerializeField] int criticalRate = 0;
        [SerializeField] int criticalDamage = 0;
        #endregion


        void Start()
        {
            Initialize();
            LevelUp();
        }

        #region Private Methods
        void Initialize()
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

        void LevelUp()
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

        void RankUp()
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
                default:
                    break;
            }
        }

        void AssignAttribute()
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

                default:
                    break;
            }
        }

        void IncreaseStrength()
        {
            strength++;
            physicalAttack += 3;
            if (characterClass == CharacterClasses.NONE && strength > 10)
            {
                physicalAttack += 20;
                characterClass = CharacterClasses.ASSASSIN;
            }
        }

        void IncreaseWisdom()
        {
            wisdom++;
            magicalAttack += 3;
            if (characterClass == CharacterClasses.NONE && wisdom > 10)
            {
                magicalAttack += 20;
                characterClass = CharacterClasses.WARLOCK;
            }
        }

        void IncreaseEndurance()
        {
            endurance++;
            damageResist += 2;
            if (characterClass == CharacterClasses.NONE && endurance > 10)
            {
                damageResist += 15;
                characterClass = CharacterClasses.TANK;
            }
        }

        void IncreaseLuck()
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