/// <summary>
/// 2/13/18
/// Allan Murillo
/// RPG Core Project
/// CharacterMovement.cs
/// </summary>
using UnityEngine;
using UnityEngine.AI;


namespace RPG {

    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class CharacterMovement : MonoBehaviour {


        #region Properties
        [Header("Movement")]
        [SerializeField] float idleTurnSpeed = 90;
        [SerializeField] float movingTurnSpeed = 180;
        [SerializeField] float stoppingDistance = 1f;
        [SerializeField] float moveSpeedMultiplier = 1f;

        private float turnAmt;
        private float forwardAmt;

        //  References    
        Character characterMaster;
        #endregion



        void Start()
        {
            characterMaster = GetComponent<Character>();
            characterMaster.MyNavAgent.updatePosition = true;
            characterMaster.MyNavAgent.updateRotation = false;
            characterMaster.MyNavAgent.stoppingDistance = stoppingDistance;

            characterMaster.MyAnim.applyRootMotion = true;

            CameraRaycaster cameraraycaster = Camera.main.GetComponent<CameraRaycaster>();

            characterMaster.MyRigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

            //  Register an observer
            cameraraycaster.onMouseOverEnemy += OnMouseOverEnemy;
            cameraraycaster.onMouseOverTerrain += OnMouseWalkable;
        }

        void Update()
        {
            if (characterMaster.MyNavAgent.remainingDistance > characterMaster.MyNavAgent.stoppingDistance)
            {
                Move(characterMaster.MyNavAgent.desiredVelocity, false, false);
            }
            else
            {
                Move(Vector3.zero, false, false);
            }
        }

        #region Movement
        public void Move(Vector3 move, bool crouch, bool jump)
        {
            if (move.magnitude > 1f) move.Normalize();
            move = transform.InverseTransformDirection(move);
            move = Vector3.ProjectOnPlane(move, Vector3.zero);
            turnAmt = Mathf.Atan2(move.x, move.z);
            forwardAmt = move.z;

            ApplyExtraTurnRotation();

            UpdateAnimator(move);
        }

        void ApplyExtraTurnRotation()
        {
            float turnSpeed = Mathf.Lerp(idleTurnSpeed, movingTurnSpeed, forwardAmt);
            transform.Rotate(0, turnAmt * turnSpeed * Time.deltaTime, 0);
        }
        #endregion

        #region Animator
        void UpdateAnimator(Vector3 move)
        {
            characterMaster.MyAnim.SetFloat("Forward", forwardAmt, 0.1f, Time.deltaTime);
            characterMaster.MyAnim.SetFloat("Turn", turnAmt, 0.1f, Time.deltaTime);
            characterMaster.MyAnim.speed = moveSpeedMultiplier;
        }

        public void OnAnimatorMove()
        {
            if (Time.deltaTime > 0)
            {
                Vector3 velocity = (characterMaster.MyAnim.deltaPosition * moveSpeedMultiplier) / Time.deltaTime;
                velocity.y = characterMaster.MyRigid.velocity.y;
                characterMaster.MyRigid.velocity = velocity;
            }
        }
        #endregion

        #region Input Events
        void OnMouseOverEnemy(Character mob)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //characterMaster.MyNavAgent.SetDestination(mob.transform.position);
                //  TODO : Normal Attack (Fireball || Sword Swing)
                //mob.ToggleUI();
                characterMaster.CallEventSetAttackTarget(mob.transform);
            }
        }

        void OnMouseWalkable(Vector3 dest)
        {
            if (Input.GetMouseButton(0))
            {
                characterMaster.MyNavAgent.SetDestination(dest);
            }
        }
        #endregion

        #region Death
        public void Kill()
        {

        }
        #endregion
    }
}