// Allan Murillo : Unity RPG Core Test Project
using UnityEngine;


namespace RPG {

    [System.Serializable]
    public class MobMaster : Character {

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
        

        GameObject player;
        #endregion


        private void Awake()
        {
            mobTag = gameObject.tag;
            mobRank = MobRanks.MINION;
            mobClass = MobClass.NONE;

            
            player = GameObject.FindGameObjectWithTag("Player");
        }

        #region Accessors & Modifiers
        public string MobTag { get { return mobTag; } }

        public MobTypes MobType { get { return mobType; } set { mobType = value; } }
        public MobRanks MobRank { get { return mobRank; } set { mobRank = value; } }
        public MobClass MobClass { get { return mobClass; } set { mobClass = value; } }

        public bool IsOnRoute { get { return isOnRoute; } set { isOnRoute = value; } }
        public bool IsNavPaused { get { return isNavPaused; } set { isNavPaused = value; } }
        public bool IsAttacking { get { return isAttacking; } set { isAttacking = value; } }
        public bool IsCriticallyHit { get { return isCriticallyHit; } set { isCriticallyHit = value; } }

        //public Icon Thumnail { get { return mobIcon; } set { mobIcon = value; } }
       
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
    }    
}