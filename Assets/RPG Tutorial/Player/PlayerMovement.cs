using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AICharacterControl))]
[RequireComponent(typeof(ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{

    CameraRaycaster cameraraycaster;
    AICharacterControl aiCharControl;
    Vector3 currentDestination, clickPoint;
    ThirdPersonCharacter thirdPersonCharacter;

    GameObject walkTarget = null;

    [SerializeField] const int walkableLayerNumber = 8, enemyLayerNumber = 9;

    bool inputMode = false;   


    // Use this for initialization
    void Start()
    {
        cameraraycaster = Camera.main.GetComponent<CameraRaycaster>();
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        currentDestination = transform.position;
        aiCharControl = GetComponent<AICharacterControl>();

        walkTarget = new GameObject("walkTarget");

        //  Register an observer
        cameraraycaster.notifyMouseClickObservers += ProcessMouseClick;
    }

    //  TODO : make this work again
    void GamepadMovement()
    {
        // read inputs
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        bool crouch = Input.GetButton("GamepadB");

        // calculate camera relative direction to move:
        Vector3 camForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 movement = v * camForward + h * Camera.main.transform.right;

        thirdPersonCharacter.Move(movement, crouch, false);
    }

    void ProcessMouseClick(RaycastHit raycastHit, int layerHit)
    {
        switch (layerHit)
        {
            case enemyLayerNumber:
                // Navigate to enemy
                GameObject enemy = raycastHit.collider.gameObject;
                aiCharControl.SetTarget(enemy.transform);
                break;
            case walkableLayerNumber:
                // Navigate to point on ground
                walkTarget.transform.position = raycastHit.point;
                aiCharControl.SetTarget(walkTarget.transform);
                break;

            default:
                Debug.LogWarning("Dont know how to handle mouse click");
                return;
        }
    }
}