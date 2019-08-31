/// <summary>
/// 2/13/18
/// Allan Murillo
/// RPG Core Project
/// MobNavDestination.cs
/// </summary>
using UnityEngine;


namespace RPG.Stats
{
    public class MobNavDestination : MonoBehaviour
    {
        Character mobMaster;

        private float checkRate;
        private float nextCheck;


        void Initialize()
        {
            mobMaster = GetComponent<Character>();
            checkRate = Random.Range(0.3f, 0.4f);
        }

        void OnEnable()
        {
            Initialize();
            mobMaster.EventCharacterDie += DisableThis;
        }

        void OnDisable()
        {
            mobMaster.EventCharacterDie -= DisableThis;
        }

        void Update()
        {
            if (Time.time > nextCheck)
            {
                nextCheck = Time.time + checkRate;
                CheckIfDestinationReached();
            }
        }

        #region Check Destination
        /// <summary>
        /// 
        /// </summary>
        void CheckIfDestinationReached()
        {
            if (mobMaster.IsOnRoute)
            {
                if (mobMaster.MyNavAgent.remainingDistance < mobMaster.MyNavAgent.stoppingDistance)
                {
                    mobMaster.IsOnRoute = false;
                    mobMaster.CallEventCharacterReachedNavTarget();
                }
            }
        }
        /// <summary>
        /// Disables Upon Death
        /// </summary>
        void DisableThis()
        {
            this.enabled = false;
        }
        #endregion
    }
}