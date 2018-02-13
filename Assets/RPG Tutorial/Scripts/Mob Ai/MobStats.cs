// Allan Murillo : Unity RPG Core Test Project
using UnityEngine;


namespace RPG {

    [System.Serializable]
    [RequireComponent(typeof(MobMaster))]
    public class MobStats : MonoBehaviour {

        #region Properties
        MobMaster mobMaster;
        [Header("Mob Info")]
        [SerializeField] int mobLevel;

        [Header("Mob Experience")]
        [SerializeField] float mobExperience;
        [SerializeField] float mobNextLvlExp;
        [SerializeField] float mobExpMult;

        [Header("Mob Stats")]
        [SerializeField] int mobStrength;
        [SerializeField] int mobWisdom;
        [SerializeField] int mobEndurance;
        [SerializeField] int mobLuck;

        [Header("Mob Characteristics")]
        [SerializeField] int physicalAttack;
        [SerializeField] int magicalAttack;
        [SerializeField] int damageResist;
        [SerializeField] int criticalRate;
        [SerializeField] int criticalDamage;        
        #endregion



        void Initialize()
        {
            mobMaster = GetComponent<MobMaster>();

            mobLevel = 1;
            mobExperience = 0f;
            mobNextLvlExp = 1f;
            mobExpMult = 1f;

            mobStrength = 0;
            mobWisdom = 0;
            mobEndurance = 0;
            mobLuck = 0;

            physicalAttack = Random.Range(1, 10);
            magicalAttack = Random.Range(1, 10);
            damageResist = Random.Range(0, 5);
            criticalRate = Random.Range(0, 5);
            criticalDamage = Random.Range(1, 10);
        }

        void OnEnable()
        {
            Initialize();
            mobMaster.EventCharacterGainExperience += ExperienceUp;
        }

        void OnDisable()
        {
            mobMaster.EventCharacterGainExperience -= ExperienceUp;
        }

        #region Accessors & Modifiers
        public int Level { get { return mobLevel; } }

        public void ExperienceUp(float exp)
        {
            mobExperience += exp * mobExpMult;
            if (mobExperience > mobNextLvlExp)
            {
                LevelUp();
                mobExperience = 0f;
                mobNextLvlExp += 1f;
            }
        }

        public void LevelUp()
        {
            mobLevel++;

            if (mobLevel < 1)
                mobLevel = 1;
            else if (mobLevel > 30)
                mobLevel = 30;
            else
            {
                if (mobMaster.MobTag == "Enemy")
                {
                    AssignAttribute();
                    physicalAttack += 1;
                    magicalAttack += 1;
                    damageResist += 1;
                }
                mobMaster.RankUp(mobLevel);
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
            mobStrength++;
            physicalAttack += 3;
            if (mobStrength == 10)
            {
                physicalAttack += 20;
                mobMaster.MobClass = RPG.MobClass.ASSASSIN;
            }
        }
        void IncreaseWisdom()
        {
            mobWisdom++;
            magicalAttack += 3;
            if (mobWisdom == 10)
            {
                magicalAttack += 20;
                mobMaster.MobClass = RPG.MobClass.WARLOCK;
            }
        }
        void IncreaseEndurance()
        {
            mobEndurance++;
            damageResist += 2;
            if (mobEndurance == 10)
            {
                damageResist += 15;
                mobMaster.MobClass = RPG.MobClass.TANK;
            }
        }
        void IncreaseLuck()
        {
            mobLuck++;
            criticalRate += 1;
            criticalDamage += 5;
            mobExpMult += 0.2f;
        }

        public int PhysicalDamage { get { return physicalAttack; } }
        public int MagicalDamage { get { return magicalAttack; } }
        public int DamageResist { get { return damageResist; } }
        public int CriticalRate { get { return criticalRate; } }
        public int CriticalDamage { get { return criticalDamage; } }
        #endregion
    }
}