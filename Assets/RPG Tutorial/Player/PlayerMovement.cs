using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;


[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AICharacterControl))]
[RequireComponent(typeof(ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour {


    CameraRaycaster cameraraycaster;
    AICharacterControl aiCharControl;
    Vector3 currentDestination, clickPoint;
    ThirdPersonCharacter thirdPersonCharacter;
    GameObject walkTarget = null;



    void Start()
    {
        cameraraycaster = Camera.main.GetComponent<CameraRaycaster>();
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        currentDestination = transform.position;
        aiCharControl = GetComponent<AICharacterControl>();

        walkTarget = new GameObject("walkTarget");

        //  Register an observer
        cameraraycaster.onMouseOverEnemy += OnMouseOverEnemy;
        cameraraycaster.onMouseOverTerrain += OnMouseWalkable;
    }

    void OnMouseOverEnemy(Enemy enemy)
    {
        if (Input.GetMouseButtonDown(0))
        {
            aiCharControl.SetTarget(enemy.transform);
        }
    }

    void OnMouseWalkable(Vector3 dest)
    {
        if (Input.GetMouseButton(0))
        {
            walkTarget.transform.position = dest;
            aiCharControl.SetTarget(walkTarget.transform);
        }
    }
}