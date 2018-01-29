// Allan Murillo : Unity RPG Core Test Project
using UnityEngine;


namespace RPG {

	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(Animator))]
	public class ThirdPersonCharacter : MonoBehaviour {


        #region Properties
        [SerializeField] float movingTurnSpeed = 180;
		[SerializeField] float idleTurnSpeed = 90;
		[SerializeField] float runCycleOffset = 0.2f;

		float turnAmt;
		float forwardAmt;
		Vector3 groundNormal;
		Rigidbody myRigidbody;
		Animator myAnimator;
        #endregion



        void Start()
		{
			myAnimator = GetComponent<Animator>();
			myRigidbody = GetComponent<Rigidbody>();

            myAnimator.applyRootMotion = true;
			myRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
		}

		public void Move(Vector3 move, bool crouch, bool jump)
		{
			if (move.magnitude > 1f) move.Normalize();
			move = transform.InverseTransformDirection(move);
			move = Vector3.ProjectOnPlane(move, groundNormal);
			turnAmt = Mathf.Atan2(move.x, move.z);
			forwardAmt = move.z;

			ApplyExtraTurnRotation();

			UpdateAnimator(move);
		}    

		void UpdateAnimator(Vector3 move)
		{
			myAnimator.SetFloat("Forward", forwardAmt, 0.1f, Time.deltaTime);
			myAnimator.SetFloat("Turn", turnAmt, 0.1f, Time.deltaTime);

			float runCycle =
				Mathf.Repeat(
					myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime + runCycleOffset, 1);
		}

		void ApplyExtraTurnRotation()
		{
			float turnSpeed = Mathf.Lerp(idleTurnSpeed, movingTurnSpeed, forwardAmt);
			transform.Rotate(0, turnAmt * turnSpeed * Time.deltaTime, 0);
		}

		public void OnAnimatorMove()
		{
			if (Time.deltaTime > 0)
			{
				Vector3 v = myAnimator.deltaPosition / Time.deltaTime;
				v.y = myRigidbody.velocity.y;
				myRigidbody.velocity = v;
			}
		}
	}
}