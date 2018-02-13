// Allan Murillo : Unity RPG Core Test Project
using UnityEngine;
using UnityEngine.AI;


namespace RPG
{
    public class MobNavDestination : MonoBehaviour
    {

        private MobMaster mobMaster;
        private NavMeshAgent myNavMeshAgent;

        private float checkRate;
        private float nextCheck;


        void Initialize()
        {
            mobMaster = GetComponent<MobMaster>();
            if (GetComponent<NavMeshAgent>() != null)
            {
                myNavMeshAgent = GetComponent<NavMeshAgent>();
            }
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

        void CheckIfDestinationReached()
        {
            if (mobMaster.IsOnRoute)
            {
                if (myNavMeshAgent.remainingDistance < myNavMeshAgent.stoppingDistance)
                {
                    mobMaster.IsOnRoute = false;
                    mobMaster.CallEventCharacterReachedNavTarget();
                }
            }
        }

        void DisableThis()
        {
            this.enabled = false;
        }
    }
}