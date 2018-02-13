// Allan Murillo : Unity RPG Core Test Project
using UnityEngine;
using UnityEngine.AI;


namespace RPG
{
    public class MobChase : MonoBehaviour
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
            checkRate = Random.Range(0.1f, 0.2f);
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
                TryChaseTarget();
            }
        }

        void TryChaseTarget()
        {
            if (mobMaster.AttackTarget != null && myNavMeshAgent != null && !mobMaster.IsNavPaused)
            {
                myNavMeshAgent.SetDestination(mobMaster.AttackTarget.position);

                if (myNavMeshAgent.remainingDistance > myNavMeshAgent.stoppingDistance)
                {
                    mobMaster.CallEventCharacterWalking();
                    mobMaster.IsOnRoute = true;
                }
            }
        }

        void DisableThis()
        {
            if (myNavMeshAgent != null)
            {
                myNavMeshAgent.enabled = false;
            }
            this.enabled = false;
        }
    }
}