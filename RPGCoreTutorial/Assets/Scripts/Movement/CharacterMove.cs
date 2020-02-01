using ANM.Attributes;
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

        private Health _myHealth;
        private NavMeshAgent _navMeshAgent;
        private static readonly int ForwardSpeed = Animator.StringToHash("ForwardSpeed");


        private void Awake()
        {
            _myHealth = GetComponent<Health>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            _navMeshAgent.enabled = !_myHealth.IsDead();
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
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }

        private void UpdateAnimator()
        {
            var velocity = _navMeshAgent.velocity;
            var localVelocity = transform.InverseTransformDirection(velocity);
            var speed = localVelocity.z;
            GetComponent<Animator>().SetFloat(ForwardSpeed, speed);
        }

        #region Interface Methods
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
            _navMeshAgent.enabled = false;
            transform.localPosition = ((SerializableVector3)state).ToVector();
            _navMeshAgent.enabled = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }   //  ISaveable
        #endregion
    }
}