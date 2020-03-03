/*
 * CharacterMove - handles character movement and updates animations
 * ISaveable to save the characters' position in the game state
 * Created by : Allan N. Murillo
 * Last Edited : 2/26/2020
 */

using ANM.Core;
using ANM.Saving;
using UnityEngine;
using UnityEngine.AI;

namespace ANM.Movement
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class CharacterMove : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] private float maxSpeed = 5.66f;
        
        private static readonly int ForwardSpeed = Animator.StringToHash("ForwardSpeed");
        private ActionScheduler _scheduler;
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;


        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _scheduler = GetComponent<ActionScheduler>();
        }

        private void Update()
        {
            UpdateAnimator();
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            _navMeshAgent.destination = destination;
            _navMeshAgent.isStopped = false;
            _navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            _scheduler.StartAction(this);
            MoveTo(destination, speedFraction);
        }

        private void UpdateAnimator()
        {
            var velocity = _navMeshAgent.velocity;
            var localVelocity = transform.InverseTransformDirection(velocity);
            var speed = localVelocity.z;
            _animator.SetFloat(ForwardSpeed, speed);
        }

        public void Cancel()
        {
            _navMeshAgent.isStopped = true;
        }   //  IAction

        public object CaptureState()
        {
            return new SerializableVector3(transform.localPosition);
        }   //  ISaveable

        public void RestoreState(object state)
        {
            if (_scheduler == null)
            {
                Debug.Log("CharacterMove::Restoring State Action Scheduler not found for : " 
                          + gameObject.name);
                return;
            }
            _scheduler.CancelCurrentAction();
            _navMeshAgent.enabled = false;
            transform.position = ((SerializableVector3) state).ToVector();
            _navMeshAgent.enabled = true;
        }   //  ISaveable
    }
}