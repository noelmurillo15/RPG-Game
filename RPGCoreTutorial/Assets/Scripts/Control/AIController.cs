/*
 * AIController - 
 * Created by : Allan N. Murillo
 * Last Edited : 2/25/2020
 */

using ANM.Attributes;
using ANM.Combat;
using ANM.Core;
using ANM.Movement;
using GameDevTV.Utils;
using UnityEngine;

namespace ANM.Control
{
    public class AIController : MonoBehaviour
    {
        #region  AIController Class Members      
        //  Serialized Fields  
        [SerializeField] private PatrolPath patrolPath = null;
        [SerializeField] private float chaseDistance = 5f;
        [SerializeField] private float suspicionTime = 3f;
        [SerializeField] private float waypointTolerance = 0.2f;
        [SerializeField] private float waypointDwellTime = 5f;
        [SerializeField] [Range(0, 1)] private float patrolSpeedFraction = 0.2f;

        //  Cached Variables
        private Fighter _fighter;
        private Health _myHealth;
        private GameObject _player;
        private CharacterMove _characterMove;

        //  My Variables
        private LazyValue<Vector3> _guardLocation;
        private float _timeSinceLastSawPlayer = Mathf.Infinity;
        private float _timeSinceArrivedAtWaypoint = Mathf.Infinity;
        private int _waypointIndex = 0;
        #endregion


        private void Awake()
        {
            _fighter = GetComponent<Fighter>();
            _myHealth = GetComponent<Health>();
            _characterMove = GetComponent<CharacterMove>();
            _player = GameObject.FindGameObjectWithTag("Player");
            _guardLocation = new LazyValue<Vector3>(GetGuardPosition);
        }

        private void Start()
        {
            _guardLocation.ForceInit();
        }

        private void Update()
        {
            if (_myHealth.IsDead()) return;

            if (InAttackRangeOfPlayer() && Fighter.CanAttack(_player))
            {   //  Attack State
                _timeSinceLastSawPlayer = 0f;
                AttackBehaviour();
            }
            else if (_timeSinceLastSawPlayer < suspicionTime)
            {   //  Suspicion State
                SuspicionBehaviour();
            }
            else
            {   //  Idle State
                _fighter.Cancel();
                PatrolBehaviour();
            }

            UpdateTimers();
        }

        private void UpdateTimers()
        {
            _timeSinceArrivedAtWaypoint += Time.deltaTime;
            _timeSinceLastSawPlayer += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            var nextPosition = _guardLocation.value;

            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    _timeSinceArrivedAtWaypoint = 0f;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }

            if (_timeSinceArrivedAtWaypoint > waypointDwellTime)
            {
                _characterMove.MoveTo(nextPosition, patrolSpeedFraction);
            }
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            _fighter.Attack(_player);
        }

        private void CycleWaypoint()
        {
            _waypointIndex = patrolPath.GetNextIndex(_waypointIndex);
        }

        private bool AtWaypoint()
        {
            var distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }

        private bool InAttackRangeOfPlayer()
        {
            return Vector3.Distance(_player.transform.position, transform.position) < chaseDistance;
        }

        private Vector3 GetGuardPosition()
        {
            return transform.localPosition;
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(_waypointIndex);
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