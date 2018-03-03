/// <summary>
/// 2/13/18
/// Allan Murillo
/// RPG Core Project
/// CharacterMovement.cs
/// </summary>
using UnityEngine;


namespace RPG {

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

            characterMaster.MyRigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        }

        void Update()
        {
            if (characterMaster.MyNavAgent.remainingDistance > characterMaster.MyNavAgent.stoppingDistance)
            {
                Move(characterMaster.MyNavAgent.desiredVelocity);
            }
            else
            {
                Move(Vector3.zero);
            }
        }

        #region Movement
        public void Move(Vector3 move)
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
    }
}