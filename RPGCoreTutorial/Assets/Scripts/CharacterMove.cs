using RPG.Core;
using RPG.Saving;
using UnityEngine;
using UnityEngine.AI;


namespace RPG.Movement
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class CharacterMove : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] float maxSpeed = 5.66f;

        NavMeshAgent navMeshAgent;
        Health myHealth;


        void Start()
        {
            myHealth = GetComponent<Health>();
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            navMeshAgent.enabled = !myHealth.IsDead();
            UpdateAnimator();
        }

        public void MoveTo(Vector3 _destination, float _speedFraction)
        {
            navMeshAgent.destination = _destination;
            navMeshAgent.isStopped = false;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(_speedFraction);
        }

        public void StartMoveAction(Vector3 _destination, float _speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(_destination, _speedFraction);
        }

        void UpdateAnimator()
        {
            Vector3 m_velocity = navMeshAgent.velocity;
            Vector3 m_localVelocity = transform.InverseTransformDirection(m_velocity);
            float m_speed = m_localVelocity.z;
            GetComponent<Animator>().SetFloat("ForwardSpeed", m_speed);
        }

        #region Interface Methods
        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            GetComponent<NavMeshAgent>().enabled = false;
            SerializableVector3 position = (SerializableVector3)state;
            transform.position = position.ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
        }
        #endregion
    }
}