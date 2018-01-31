// Allan Murillo : Unity RPG Core Test Project
using UnityEngine;


namespace RPG {

    [System.Serializable]
    public class MobMaster : MonoBehaviour {

        #region Properties
        [SerializeField] string mobTag;
        [SerializeField] MobTypes mobType;
        [SerializeField] MobRanks mobRank;
        [SerializeField] MobClass mobClass;

        [SerializeField] bool isOnRoute;
        [SerializeField] bool isNavPaused;
        [SerializeField] bool isAttacking;
        [SerializeField] bool isCriticallyHit;

        //[SerializeField] Icon mobIcon;
        [SerializeField] Transform mobAttackTarget;

        GameObject player;
        HealthSystem myHealth;
        #endregion

        #region Events
        //  Delegates
        public delegate void GeneralEventHandler();
        public delegate void StatsEventHandler(int hp);
        public delegate void NavTargetEventHandler(Transform targetTransform);

        //  Animation Events
        public event GeneralEventHandler EventMobDie;
        public event GeneralEventHandler EventMobWalking;
        public event GeneralEventHandler EventMobReachedNavTarget;
        public event GeneralEventHandler EventMobAttack;
        public event GeneralEventHandler EventMobLostTarget;
        //  Stat Events
        public event StatsEventHandler EventMobDeductHealth;
        public event StatsEventHandler EventMobAddExperience;
        //  Nav Events
        public event NavTargetEventHandler EventMobSetNavTarget;
        #endregion



        private void Awake()
        {
            mobTag = gameObject.tag;
            mobRank = MobRanks.MINION;
            mobClass = MobClass.NONE;

            myHealth = GetComponent<HealthSystem>();
            player = GameObject.FindGameObjectWithTag("Player");
        }

        #region Accessors & Modifiers
        public string MobTag { get { return mobTag; } }
        public HealthSystem MobHealthSystem { get{ return myHealth; } }

        public MobTypes MobType { get { return mobType; } set { mobType = value; } }
        public MobRanks MobRank { get { return mobRank; } set { mobRank = value; } }
        public MobClass MobClass { get { return mobClass; } set { mobClass = value; } }

        public bool IsOnRoute { get { return isOnRoute; } set { isOnRoute = value; } }
        public bool IsNavPaused { get { return isNavPaused; } set { isNavPaused = value; } }
        public bool IsAttacking { get { return isAttacking; } set { isAttacking = value; } }
        public bool IsCriticallyHit { get { return isCriticallyHit; } set { isCriticallyHit = value; } }

        //public Icon Thumnail { get { return mobIcon; } set { mobIcon = value; } }
        public Transform MobTarget { get { return mobAttackTarget; } set { mobAttackTarget = value; } }

        public void RankUp(int myLevel)
        {
            switch (myLevel)
            {
                case 6:
                    MobRank = MobRanks.COMMON;
                    break;
                case 11:
                    MobRank = MobRanks.ELITE;
                    break;
                case 16:
                    MobRank = MobRanks.LEGENDARY;
                    break;
                case 21:
                    MobRank = MobRanks.UBER;
                    break;
                case 26:
                    MobRank = MobRanks.BOSS;
                    break;
                case 30:
                    Debug.Log(mobTag + " has achieved God Rank!");
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Events
        public void CallEventMobDeductHealth(int hp)
        {
            if (EventMobDeductHealth != null)
            {
                EventMobDeductHealth(hp);
            }
        }
        public void CallEventMobExpUp(int exp)
        {
            if (EventMobAddExperience != null)
            {
                EventMobAddExperience(exp);
            }
        }
        public void CallEventMobSetNavTarget(Transform targetTransform)
        {
            if (EventMobSetNavTarget != null)
            {
                EventMobSetNavTarget(targetTransform);
            }

            mobAttackTarget = targetTransform;
        }

        public void CallEventMobDie()
        {
            if (EventMobDie != null)
            {
                EventMobDie();
            }
        }

        public void CallEventMobWalking()
        {
            if (EventMobWalking != null)
            {
                EventMobWalking();
            }
        }

        public void CallEventMobReachedNavTarget()
        {
            if (EventMobReachedNavTarget != null)
            {
                EventMobReachedNavTarget();
            }
        }

        public void CallEventMobAttack()
        {
            if (EventMobAttack != null)
            {
                EventMobAttack();
            }
        }

        public void CallEventMobLostTarget()
        {
            if (EventMobLostTarget != null)
            {
                EventMobLostTarget();
            }
            mobAttackTarget = null;
        }
        #endregion
    }    
}