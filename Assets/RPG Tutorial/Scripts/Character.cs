/// <summary>
/// 2/13/18
/// Allan Murillo
/// RPG Core Project
/// Character.cs
/// </summary>
using UnityEngine;
using UnityEngine.AI;


namespace RPG {

    [System.Serializable]
    public class Character : MonoBehaviour {


        #region Properties
        [Header("Character")]
        [SerializeField] int myLevel;
        [SerializeField] string myTag;
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

        [Header("Behaviours")]
        [SerializeField] bool isOnRoute;
        [SerializeField] bool isNavPaused;
        [SerializeField] bool isAttacking;
        [SerializeField] bool isCriticallyHit;

        [Header("References")]
        [SerializeField] Transform myAttackTarget;
        protected Animator myAnimator;
        protected Transform myTransform;
        protected Rigidbody myRigidbody;
        protected NavMeshAgent myNavMeshAgent;
        #endregion

        #region Events
        public delegate void GeneralEventHandler();
        public delegate void StatsEventHandler(float hp);
        public delegate void NavTargetEventHandler(Transform targetTransform);

        public event GeneralEventHandler EventCharacterDie;
        public event GeneralEventHandler EventCharacterAttack;
        public event GeneralEventHandler EventCharacterWalking;
        public event GeneralEventHandler EventCharacterLostTarget;
        public event GeneralEventHandler EventCharacterReachedNavTarget;

        public event StatsEventHandler EventCharacterHeal;
        public event StatsEventHandler EventCharacterTakeDamage;
        public event StatsEventHandler EventCharacterGainExperience;

        public event NavTargetEventHandler EventSetAttackTarget;
        public event NavTargetEventHandler EventSetCharacterNavTarget;
        #endregion



        #region Public Methods
        public int Level { get { return myLevel; } }
        public string TagReference { get { return myTag; } }
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

        public bool IsOnRoute { get { return isOnRoute; } set { isOnRoute = value; } }
        public bool IsNavPaused { get { return isNavPaused; } set { isNavPaused = value; } }
        public bool IsAttacking { get { return isAttacking; } set { isAttacking = value; } }
        public bool IsCriticallyHit { get { return isCriticallyHit; } set { isCriticallyHit = value; } }

        public Transform MyTransformRef { get { return myTransform; } }
        public NavMeshAgent MyNavAgent { get { return myNavMeshAgent; } }
        public Rigidbody MyRigid { get { return myRigidbody; } }
        public Animator MyAnim { get { return myAnimator; } }
        public Transform AttackTarget { get { return myAttackTarget; } set { myAttackTarget = value; } }

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
        #endregion

        #region Protected Methods
        protected void Initialize()
        {
            myLevel = 0;
            myTag = gameObject.tag;            
            if (myTag == "Mob")
            {
                characterType = CharacterTypes.ENEMY;
            }
            else if (myTag == "Npc")
            {
                characterType = CharacterTypes.NPC;
            }
            if (myTag == "Player")
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
            LevelUp();

            myTransform = transform;
            myAnimator = GetComponent<Animator>();
            myRigidbody = GetComponent<Rigidbody>();
            myNavMeshAgent = GetComponent<NavMeshAgent>();
        }
        #endregion

        #region Private Methods
        void LevelUp()
        {
            myLevel++;
            myLevel = Mathf.Clamp(myLevel, 0, 30);

            if (TagReference == "Enemy")
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
                    Debug.Log(myTag + " has achieved God Rank!");
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
            if (strength == 10)
            {
                physicalAttack += 20;
                characterClass = CharacterClasses.ASSASSIN;
            }
        }

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

        void IncreaseLuck()
        {
            luck++;
            criticalRate += 1;
            criticalDamage += 5;
            expMultiplier += 0.2f;
        }
        #endregion

        #region Event Methods
        /// <summary>
        /// 
        /// </summary>
        public void CallEventCharacterDie()
        {
            if (EventCharacterDie != null)
            {
                EventCharacterDie();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void CallEventCharacterAttack()
        {
            if (EventCharacterAttack != null)
            {
                EventCharacterAttack();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void CallEventCharacterWalking()
        {
            if (EventCharacterWalking != null)
            {
                EventCharacterWalking();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void CallEventCharacterLostTarget()
        {
            if (EventCharacterLostTarget != null)
            {
                EventCharacterLostTarget();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void CallEventCharacterReachedNavTarget()
        {
            if (EventCharacterReachedNavTarget != null)
            {
                EventCharacterReachedNavTarget();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hp"></param>
        public void CallEventCharacterHeal(float hp)
        {
            if (EventCharacterHeal != null)
            {
                EventCharacterHeal(hp);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hp"></param>
        public void CallEventCharacterTakeDamage(float hp)
        {
            if (EventCharacterTakeDamage != null)
            {
                EventCharacterTakeDamage(hp);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="exp"></param>
        public void CallEventCharacterGainExperience(float exp)
        {
            if (EventCharacterGainExperience != null)
            {
                ExperienceUp(exp);
                EventCharacterGainExperience(exp);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetTransform"></param>
        public void CallEventSetAttackTarget(Transform targetTransform)
        {
            if (EventSetAttackTarget != null)
            {
                AttackTarget = targetTransform;
                EventSetAttackTarget(AttackTarget);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetTransform"></param>
        public void CallEventSetCharacterNavTarget(Transform targetTransform)
        {
            if (EventSetCharacterNavTarget != null)
            {
                myNavMeshAgent.SetDestination(targetTransform.position);
                EventSetCharacterNavTarget(targetTransform);
            }
        }
        #endregion
    }
}