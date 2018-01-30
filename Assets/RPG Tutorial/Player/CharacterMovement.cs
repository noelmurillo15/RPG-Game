// Allan Murillo : Unity RPG Core Test Project
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
public class CharacterMovement : MonoBehaviour {


    #region Properties
    //  Movement System
    [SerializeField] float idleTurnSpeed = 90;
    [SerializeField] float movingTurnSpeed = 180;
    [SerializeField] float stoppingDistance = 1f;
    [SerializeField] float moveSpeedMultiplier = 1f;

    float turnAmt;
    float forwardAmt;

    //  References
    NavMeshAgent myAgent;
    Rigidbody myRigidbody;
    Animator myAnimator;
    #endregion



    void Start()
    {
        myAgent = GetComponent<NavMeshAgent>();
        myAgent.updatePosition = true;
        myAgent.updateRotation = false;
        myAgent.stoppingDistance = stoppingDistance;

        myAnimator = GetComponent<Animator>();
        myAnimator.applyRootMotion = true;

        CameraRaycaster cameraraycaster = Camera.main.GetComponent<CameraRaycaster>();

        myRigidbody = GetComponent<Rigidbody>();
        myRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        //  Register an observer
        cameraraycaster.onMouseOverEnemy += OnMouseOverEnemy;
        cameraraycaster.onMouseOverTerrain += OnMouseWalkable;
    }

    void Update()
    {
        if (myAgent.remainingDistance > myAgent.stoppingDistance)
        {
            Move(myAgent.desiredVelocity, false, false);
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
        myAnimator.SetFloat("Forward", forwardAmt, 0.1f, Time.deltaTime);
        myAnimator.SetFloat("Turn", turnAmt, 0.1f, Time.deltaTime);
        myAnimator.speed = moveSpeedMultiplier;
    }

    public void OnAnimatorMove()
    {
        if (Time.deltaTime > 0)
        {
            Vector3 velocity = (myAnimator.deltaPosition * moveSpeedMultiplier) / Time.deltaTime;
            velocity.y = myRigidbody.velocity.y;
            myRigidbody.velocity = velocity;
        }
    }
    #endregion

    #region Input Events
    void OnMouseOverEnemy(Enemy enemy)
    {
        if (Input.GetMouseButtonDown(0))
        {
            myAgent.SetDestination(enemy.transform.position);
        }
    }

    void OnMouseWalkable(Vector3 dest)
    {
        if (Input.GetMouseButton(0))
        {
            myAgent.SetDestination(dest);
        }
    }
    #endregion

    #region Death
    public void Kill()
    {

    }
    #endregion
}