using RPG.Core;
using RPG.Combat;
using UnityEngine;
using RPG.Movement;
using RPG.Attributes;
using GameDevTV.Utils;


namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        #region  AIController Class Members      
        //  Serialized Fields  
        [SerializeField] PatrolPath patrolPath = null;
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 3f;
        [SerializeField] float waypointTolerance = 0.2f;
        [SerializeField] float waypointDwellTime = 5f;
        [SerializeField] [Range(0, 1)] float patrolSpeedFraction = 0.2f;

        //  Cached Variables
        Fighter fighter;
        Health myHealth;
        GameObject player;
        CharacterMove characterMove;

        //  My Variables
        LazyValue<Vector3> guardLocation;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        int waypointIndex = 0;
        #endregion


        private void Awake()
        {
            fighter = GetComponent<Fighter>();
            myHealth = GetComponent<Health>();
            characterMove = GetComponent<CharacterMove>();
            player = GameObject.FindGameObjectWithTag("Player");
            guardLocation = new LazyValue<Vector3>(GetGuardPosition);
        }

        private void Start()
        {
            guardLocation.ForceInit();
        }

        private void Update()
        {
            if (myHealth.IsDead()) return;

            if (InAttackRangeOfPlayer() && Fighter.CanAttack(player))
            {   //  Attack State
                timeSinceLastSawPlayer = 0f;
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {   //  Suspicion State
                SuspicionBehaviour();
            }
            else
            {   //  Idle State
                fighter.Cancel();
                PatrolBehaviour();
            }

            UpdateTimers();
        }

        private void UpdateTimers()
        {
            timeSinceArrivedAtWaypoint += Time.deltaTime;
            timeSinceLastSawPlayer += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardLocation.value;

            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    timeSinceArrivedAtWaypoint = 0f;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }

            if (timeSinceArrivedAtWaypoint > waypointDwellTime)
            {
                characterMove.MoveTo(nextPosition, patrolSpeedFraction);
            }
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            fighter.Attack(player);
        }

        private void CycleWaypoint()
        {
            waypointIndex = patrolPath.GetNextIndex(waypointIndex);
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }

        private bool InAttackRangeOfPlayer()
        {
            return Vector3.Distance(player.transform.position, transform.position) < chaseDistance;
        }

        private Vector3 GetGuardPosition()
        {
            return transform.localPosition;
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(waypointIndex);
        }

        #region Custom Gizmos
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
        #endregion
    }
}