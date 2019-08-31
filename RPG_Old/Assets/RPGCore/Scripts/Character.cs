/// <summary>
/// 2/13/18
/// Allan Murillo
/// RPG Core Project
/// Character.cs
/// </summary>
using UnityEngine;
using UnityEngine.AI;


namespace RPG.Stats
{
    [SelectionBase]
    [System.Serializable]
    public class Character : MonoBehaviour
    {
        #region Properties
        [Header("Character")]
        [SerializeField] string myTag;

        [Header("Rigidbody")]
        [SerializeField]
        int rigidbodyMass;

        [Header("Capsule Collider")]
        [SerializeField] Vector3 colliderCenter;
        [SerializeField] float colliderRadius;
        [SerializeField] float colliderHeight;

        [Header("NavMesh Agent")]
        [SerializeField] float steeringSpeed = 1f;
        [SerializeField] float stoppingDistance = 2f;

        [Header("Behaviours")]
        [SerializeField] bool isOnRoute;
        [SerializeField] bool isNavPaused;
        [SerializeField] bool isAttacking;
        [SerializeField] bool isCriticallyHit;

        [Header("References")]
        Animator myAnimator;
        Transform myTransform;
        Rigidbody myRigidbody;
        NavMeshAgent myNavMeshAgent;
        [SerializeField] protected Transform myAttackTarget;
        #endregion

        #region Events
        public delegate void GeneralEventHandler();
        public delegate void StatsEventHandler(float hp);
        public delegate void NavTargetEventHandler(Transform targetTransform);

        public event GeneralEventHandler EventCharacterDie;
        public event GeneralEventHandler EventCharacterAttack;
        public event GeneralEventHandler EventCharacterWalking;
        public event GeneralEventHandler EventCharacterLostTarget;
        public event GeneralEventHandler EventCharacterReachedNavTarget;

        public event StatsEventHandler EventCharacterHeal;
        public event StatsEventHandler EventCharacterTakeDamage;

        public event NavTargetEventHandler EventSetAttackTarget;
        public event NavTargetEventHandler EventSetCharacterNavTarget;
        #endregion


        void Awake()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes all Character Components
        /// </summary>
        void Initialize()
        {
            isOnRoute = false;
            isNavPaused = false;
            isAttacking = false;
            isCriticallyHit = false;

            myTag = gameObject.tag;
            myTransform = transform;
            myAnimator = GetComponent<Animator>();

            myRigidbody = gameObject.AddComponent<Rigidbody>();
            myRigidbody.mass = rigidbodyMass;

            myNavMeshAgent = gameObject.AddComponent<NavMeshAgent>();
            myNavMeshAgent.stoppingDistance = stoppingDistance;
            myNavMeshAgent.speed = steeringSpeed;
            myNavMeshAgent.updateRotation = true;
            myNavMeshAgent.updatePosition = true;
            myNavMeshAgent.autoBraking = true;
            myNavMeshAgent.radius = colliderRadius;
            myNavMeshAgent.height = colliderHeight;

            var capCollider = gameObject.AddComponent<CapsuleCollider>();
            capCollider.center = colliderCenter;
            capCollider.radius = colliderRadius;
            capCollider.height = colliderHeight;
        }

        #region Accessors & Modifiers
        public string TagReference { get { return myTag; } }

        public bool IsOnRoute { get { return isOnRoute; } set { isOnRoute = value; } }
        public bool IsNavPaused { get { return isNavPaused; } set { isNavPaused = value; } }
        public bool IsAttacking { get { return isAttacking; } set { isAttacking = value; } }
        public bool IsCriticallyHit { get { return isCriticallyHit; } set { isCriticallyHit = value; } }

        public Transform MyTransformRef { get { return myTransform; } }
        public NavMeshAgent MyNavAgent { get { return myNavMeshAgent; } }
        public Rigidbody MyRigid { get { return myRigidbody; } }
        public Animator MyAnim { get { return myAnimator; } }
        public Transform AttackTarget { get { return myAttackTarget; } }
        #endregion

        #region Event Functions
        /// <summary>
        /// Called by Health System
        /// Disables Most Mob Ai Scripts
        /// </summary>
        public void CallEventCharacterDie()
        {
            if (EventCharacterDie != null)
            {
                EventCharacterDie();
            }
        }
        /// <summary>
        /// Called by Mob Attack
        /// Triggers Mob Attack Animation
        /// </summary>
        public void CallEventCharacterAttack()
        {
            if (EventCharacterAttack != null)
            {
                EventCharacterAttack();
            }
        }
        /// <summary>
        /// Called by Mob Wander & Mob Chase
        /// Triggers Mob Walk Animation
        /// </summary>
        public void CallEventCharacterWalking()
        {
            if (EventCharacterWalking != null)
            {
                EventCharacterWalking();
            }
        }
        /// <summary>
        /// Called by Mob Detection & Mob Attack
        /// Sets my Attack Target to null
        /// </summary>
        //public void CallEventCharacterLostTarget()
        //{
        //    if (EventCharacterLostTarget != null)
        //    {
        //        myAttackTarget = null;
        //        EventCharacterLostTarget();
        //    }
        //}
        /// <summary>
        /// Called by MobNavDestination
        /// Triggers Mob Idle Animation
        /// </summary>
        public void CallEventCharacterReachedNavTarget()
        {
            if (EventCharacterReachedNavTarget != null)
            {
                EventCharacterReachedNavTarget();
            }
        }
        /// <summary>
        /// Called by Buff Spell Behaviour
        /// HealthSystem Triggered
        /// </summary>
        /// <param name="hp"></param>
        public void CallEventCharacterHeal(float hp)
        {
            if (EventCharacterHeal != null)
            {
                EventCharacterHeal(hp);
            }
        }
        /// <summary>
        /// Called by Mob Attack, Player Master, Spell Behaviour
        /// HealthSystem Triggered
        /// </summary>
        /// <param name="hp"></param>
        public void CallEventCharacterTakeDamage(float hp)
        {
            if (EventCharacterTakeDamage != null)
            {
                EventCharacterTakeDamage(hp);
            }
        }
        /// <summary>
        /// Called by Mob Detection
        /// Mob Attack Triggered
        /// </summary>
        /// <param name="targetTransform"></param>
        public void CallEventSetAttackTarget(Transform targetTransform)
        {
            myAttackTarget = targetTransform;
            if (EventSetAttackTarget != null)
            {
                EventSetAttackTarget(AttackTarget);
            }
        }
        /// <summary>
        /// Called by Mob Detection
        /// Mob Chase Triggered
        /// </summary>
        /// <param name="targetTransform"></param>
        public void CallEventSetCharacterNavTarget(Transform targetTransform)
        {
            if (EventSetCharacterNavTarget != null)
            {
                EventSetCharacterNavTarget(targetTransform);
            }
        }
        #endregion
    }
}