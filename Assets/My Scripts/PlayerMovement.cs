using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float walkStopRadius = 1f;
    [SerializeField] float attackStopRadius = 5f;

    ThirdPersonCharacter thirdPersonCharacter;
    private CameraRaycaster cameraraycaster;
    Vector3 currentDestination, clickPoint;
    private bool jump; 
    
    bool inputMode = false;   


    // Use this for initialization
    void Start()
    {
        cameraraycaster = Camera.main.GetComponent<CameraRaycaster>();
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        currentDestination = transform.position;
    }

    Vector3 ShortDestination(Vector3 destination, float stopRadius)
    {
        Vector3 reductionVector = (destination - transform.position).normalized * stopRadius;
        return destination - reductionVector;
    }

    private void Update()
    {
        if (!jump)
        {
            jump = Input.GetButtonDown("Jump");
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            inputMode = !inputMode;
            currentDestination = transform.position;
        }

        if (inputMode)
            MouseMovement();
        else
            GamepadMovement();

        jump = false;
    }

    private void GamepadMovement()
    {
        // read inputs
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        bool crouch = Input.GetButton("GamepadB");

        // calculate camera relative direction to move:
        Vector3 camForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 movement = v * camForward + h * Camera.main.transform.right;

        thirdPersonCharacter.Move(movement, crouch, jump);
    }


    private void MouseMovement()
    {
        bool crouch = Input.GetKey(KeyCode.C);

        if (Input.GetMouseButton(0))
        {
            clickPoint = cameraraycaster.hit.point;
            switch (cameraraycaster.currentLayerHit)
            {
                case Layer.Enemy:
                    currentDestination = ShortDestination(clickPoint, attackStopRadius);
                    break;
                case Layer.Walkable:
                    currentDestination = ShortDestination(clickPoint, walkStopRadius);
                    break;


                default:
                    print("Unexpected layer found.");
                    break;
            }

            WalkToDestination(crouch);
        }
    }

    private void WalkToDestination(bool crouch)
    {
        var playerToClickPoint = currentDestination - transform.position;

        if (playerToClickPoint.magnitude >= 0)
            thirdPersonCharacter.Move(playerToClickPoint, crouch, jump);
        else
            thirdPersonCharacter.Move(Vector3.zero, crouch, jump);      
    }

    private void OnDrawGizmos()
    {
    //  TODO Learn Gizmos
        //  Movement Gizmo
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, currentDestination);
        Gizmos.DrawSphere(currentDestination, 0.1f);
        Gizmos.DrawSphere(clickPoint, 0.1f);

        //  Attack Radius Gizmo
        Gizmos.color = new Color(255f, 0f, 0f, .5f);
        Gizmos.DrawWireSphere(transform.position, attackStopRadius);
    }
}