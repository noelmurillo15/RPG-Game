// Allan Murillo : Unity RPG Core Test Project
using RPG;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(ThirdPersonCharacter))]
public class CharacterMovement : MonoBehaviour {


    [SerializeField] float stoppingDistance = 1f;

    NavMeshAgent agent;
    GameObject walkTarget;
    Vector3 clickPoint;
    ThirdPersonCharacter thirdPersonCharacter;



    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = true;
        agent.updateRotation = false;
        agent.stoppingDistance = stoppingDistance;

        walkTarget = new GameObject("walkTarget");
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        CameraRaycaster cameraraycaster = Camera.main.GetComponent<CameraRaycaster>();

        //  Register an observer
        cameraraycaster.onMouseOverEnemy += OnMouseOverEnemy;
        cameraraycaster.onMouseOverTerrain += OnMouseWalkable;
    }

    void Update()
    {
        if (agent.remainingDistance > agent.stoppingDistance)
        {
            thirdPersonCharacter.Move(agent.desiredVelocity, false, false);
        }
        else
        {
            thirdPersonCharacter.Move(Vector3.zero, false, false);
        }
    }

    void OnMouseOverEnemy(Enemy enemy)
    {
        if (Input.GetMouseButtonDown(0))
        {
            agent.SetDestination(enemy.transform.position);
        }
    }

    void OnMouseWalkable(Vector3 dest)
    {
        if (Input.GetMouseButton(0))
        {
            agent.SetDestination(dest);
        }
    }
}