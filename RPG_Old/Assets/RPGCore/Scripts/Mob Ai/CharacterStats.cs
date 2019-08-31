/// <summary>
/// 2/14/18
/// Allan Murillo
/// RPG Core Project
/// CharacterStats.cs
/// </summary>
using UnityEngine;


namespace RPG.Stats
{
    [System.Serializable]
    public class CharacterStats : MonoBehaviour
    {
        #region Variables
        [Header("Character Info")]
        [SerializeField] int myLevel;
        [SerializeField] CharacterTypes characterType;
        [SerializeField] CharacterRanks characterRank;
        [SerializeField] CharacterClasses characterClass;

        [Header("Experience")]
        [SerializeField] float currExp;
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

        [SerializeField] float meleeRange = 5f;
        [SerializeField] float magicRange = 5f;
        [SerializeField] float attackRate = .5f;

        Character characterMaster;
        #endregion


        void Start()
        {
            Initialize();
            LevelUp();
        }
        /// <summary>
        /// Initializes Character Stats
        /// </summary>
        void Initialize()
        {
            characterMaster = GetComponent<Character>();
            myLevel = 0;
            if (characterMaster.TagReference == "Mob")
            {
                characterType = CharacterTypes.ENEMY;
            }
            else if (characterMaster.TagReference == "Npc")
            {
                characterType = CharacterTypes.NPC;
            }
            if (characterMaster.TagReference == "Player")
            {
                characterType = CharacterTypes.PLAYER;
            }

            characterRank = CharacterRanks.MINION;
            characterClass = CharacterClasses.NONE;

            currExp = 0f;
            expToNextLvl = 1f;
            expMultiplier = 1f;

            strength = 0;
            wisdom = 0;
            endurance = 0;
            luck = 0;

            physicalAttack = Random.Range(1, 10);
            magicalAttack = Random.Range(1, 10);
            damageResist = Random.Range(0, 5);
            criticalRate = Random.Range(0, 5);
            criticalDamage = Random.Range(1, 10);
        }
        /// <summary>
        /// TODO : Call on character death - reward character hat dealt
        /// the last hit to trigger character death
        /// </summary>
        /// <param name="exp"></param>
        public void ExperienceUp(float exp)
        {
            currExp += exp * expMultiplier;
            if (currExp > expToNextLvl)
            {
                LevelUp();
                currExp = 0f;
                expToNextLvl += 1f;
            }
        }

        #region Private Functions
        /// <summary>
        /// 
        /// </summary>
        void LevelUp()
        {
            myLevel++;
            myLevel = Mathf.Clamp(myLevel, 0, 30);

            if (characterMaster.TagReference == "Enemy")
            {
                AssignAttribute();
                physicalAttack += 1;
                magicalAttack += 1;
                damageResist += 1;
            }
            RankUp();
        }
        /// <summary>
        /// 
        /// </summary>
        void RankUp()
        {
            switch (myLevel)
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
                    Debug.Log(characterMaster.TagReference + " has achieved God Rank!");
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 
        /// </summary>
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
        /// <summary>
        /// 
        /// </summary>
        void IncreaseStrength()
        {
            strength++;
            physicalAttack += 3;
            if (strength == 10)
            {
                physicalAttack += 20;
                characterClass = CharacterClasses.ASSASSIN;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        void IncreaseWisdom()
        {
            wisdom++;
            magicalAttack += 3;
            if (wisdom == 10)
            {
                magicalAttack += 20;
                characterClass = CharacterClasses.WARLOCK;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        void IncreaseEndurance()
        {
            endurance++;
            damageResist += 2;
            if (endurance == 10)
            {
                damageResist += 15;
                characterClass = CharacterClasses.TANK;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        void IncreaseLuck()
        {
            luck++;
            criticalRate += 1;
            criticalDamage += 5;
            expMultiplier += 0.2f;
        }
        #endregion

        #region Accessors
        public int Level { get { return myLevel; } }
        public CharacterTypes CharacterType { get { return characterType; } }
        public CharacterRanks CharacterRank { get { return characterRank; } }
        public CharacterClasses CharacterClass { get { return characterClass; } }

        public int Strength { get { return strength; } }
        public int Wisdom { get { return wisdom; } }
        public int Endurance { get { return endurance; } }
        public int Luck { get { return luck; } }

        public int PhysicalAttack { get { return physicalAttack; } }
        public int MagicalAttack { get { return magicalAttack; } }
        public int DamageResist { get { return damageResist; } }
        public int CriticalRate { get { return criticalRate; } }
        public int CriticalDamage { get { return criticalDamage; } }

        public float MeleeRange { get { return meleeRange; } }
        public float MagicRange { get { return magicRange; } }
        public float AttackRate { get { return attackRate; } }
        #endregion
    }
}