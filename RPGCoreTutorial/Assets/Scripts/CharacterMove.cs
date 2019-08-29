using RPG.Core;
using UnityEngine;
using UnityEngine.AI;


namespace RPG.Movement
{    
    [RequireComponent(typeof(NavMeshAgent))]
    public class CharacterMove : MonoBehaviour, IAction {
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

        public void MoveTo(Vector3 _destination) 
        {
            navMeshAgent.destination = _destination;
            navMeshAgent.isStopped = false;
        }

        public void StartMoveAction(Vector3 _destination) 
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(_destination);
        }

        void UpdateAnimator()
        {
            Vector3 m_velocity = navMeshAgent.velocity;
            Vector3 m_localVelocity = transform.InverseTransformDirection(m_velocity);
            float m_speed = m_localVelocity.z;
            GetComponent<Animator>().SetFloat("ForwardSpeed", m_speed);
        }

        #region Interface Methods
        public void Cancel(){
            navMeshAgent.isStopped = true;
        }
        #endregion
    }
}