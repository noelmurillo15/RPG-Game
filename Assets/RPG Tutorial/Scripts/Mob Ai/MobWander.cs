// Allan Murillo : Unity RPG Core Test Project
using UnityEngine;
using UnityEngine.AI;


namespace RPG {

    public class MobWander : MonoBehaviour {


        MobMaster mobMaster;
        NavMeshHit navHit;
        Transform myTransform;
        NavMeshAgent myNavMeshAgent;

        private float checkRate;
        private float nextCheck;

        [SerializeField] float wanderRange = 20f;
        [SerializeField] Vector3 wanderTarget;


        void Initialize()
        {
            mobMaster = GetComponent<MobMaster>();
            if (GetComponent<NavMeshAgent>() != null)
            {
                myNavMeshAgent = GetComponent<NavMeshAgent>();
            }
            checkRate = Random.Range(0.1f, 0.3f);
            myTransform = transform;
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

        void CheckWander()
        {
            if (mobMaster.AttackTarget == null && !mobMaster.IsOnRoute && !mobMaster.IsNavPaused)
            {
                if (RandomWanderTarget(myTransform.position, wanderRange, out wanderTarget))
                {
                    myNavMeshAgent.SetDestination(wanderTarget);
                    mobMaster.IsOnRoute = true;
                    mobMaster.CallEventCharacterWalking();
                }
            }
        }

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

        void DisableThis()
        {
            this.enabled = false;
        }
    }
}