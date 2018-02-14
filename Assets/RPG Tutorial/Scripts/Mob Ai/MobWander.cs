/// <summary>
/// 2/13/18
/// Allan Murillo
/// RPG Core Project
/// MobWander.cs
/// </summary>
using UnityEngine;
using UnityEngine.AI;


namespace RPG {

    public class MobWander : MonoBehaviour {


        Character mobMaster;
        Vector3 wanderTarget;
        NavMeshHit navHit;

        private float checkRate;
        private float nextCheck;
        [SerializeField] float wanderRange = 20f;



        void Initialize()
        {
            mobMaster = GetComponent<Character>();
            checkRate = Random.Range(0.1f, 0.3f);
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
                CheckWander();
            }
        }

        #region Wander
        /// <summary>
        /// 
        /// </summary>
        void CheckWander()
        {
            if (mobMaster.AttackTarget == null && !mobMaster.IsOnRoute && !mobMaster.IsNavPaused)
            {
                if (RandomWanderTarget(mobMaster.MyTransformRef.position, wanderRange, out wanderTarget))
                {
                    mobMaster.MyNavAgent.SetDestination(wanderTarget);
                    mobMaster.IsOnRoute = true;
                    mobMaster.CallEventCharacterWalking();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="center"></param>
        /// <param name="range"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        bool RandomWanderTarget(Vector3 center, float range, out Vector3 result)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * wanderRange;
            if (NavMesh.SamplePosition(randomPoint, out navHit, 1f, NavMesh.AllAreas))
            {
                result = navHit.position;
                return true;
            }

            result = center;
            return false;
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