/// <summary>
/// 2/13/18
/// Allan Murillo
/// RPG Core Project
/// MobChase.cs
/// </summary>
using UnityEngine;


namespace RPG {

    public class MobChase : MonoBehaviour {


        Character mobMaster;
        Transform chaseRef;

        private float checkRate;
        private float nextCheck;



        void Initialize()
        {
            chaseRef = null;
            mobMaster = GetComponent<Character>();
            checkRate = Random.Range(0.1f, 0.2f);
        }

        void OnEnable()
        {
            Initialize();
            mobMaster.EventCharacterDie += DisableThis;
            mobMaster.EventSetCharacterNavTarget += SetChaseTarget;
        }

        void OnDisable()
        {
            mobMaster.EventCharacterDie -= DisableThis;
            mobMaster.EventSetCharacterNavTarget -= SetChaseTarget;
        }

        void Update()
        {
            if (Time.time > nextCheck)
            {
                nextCheck = Time.time + checkRate;
                TryChaseTarget();
            }
        }

        #region Chase
        void SetChaseTarget(Transform target)
        {
            chaseRef = target;
        }
        /// <summary>
        /// 
        /// </summary>
        void TryChaseTarget()
        {
            if (chaseRef != null && mobMaster.MyNavAgent != null && !mobMaster.IsNavPaused)
            {
                if (mobMaster.MyNavAgent.remainingDistance > mobMaster.MyNavAgent.stoppingDistance)
                {
                    mobMaster.CallEventCharacterWalking();
                    mobMaster.IsOnRoute = true;
                }
            }
        }
        /// <summary>
        /// Disables Upon Death
        /// </summary>
        void DisableThis()
        {
            if (mobMaster.MyNavAgent != null)
            {
                mobMaster.MyNavAgent.enabled = false;
            }
            this.enabled = false;
        }
        #endregion
    }
}