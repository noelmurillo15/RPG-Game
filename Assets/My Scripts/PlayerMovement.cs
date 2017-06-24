using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float walkMoveStopRadius = 0.2f;
    ThirdPersonCharacter thirdPersonController;
    private CameraRaycaster cameraraycaster;
    Vector3 currentClickTarget;
    private bool jump; 
    
    bool inputMode = false;   


    // Use this for initialization
    void Start()
    {
        cameraraycaster = Camera.main.GetComponent<CameraRaycaster>();
        thirdPersonController = GetComponent<ThirdPersonCharacter>();
        currentClickTarget = transform.position;
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
            currentClickTarget = transform.position;
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
        bool crouch = Input.GetButton("GamepadX");

        // calculate camera relative direction to move:
        Vector3 m_CamForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 m_Move = v * m_CamForward + h * Camera.main.transform.right;

        thirdPersonController.Move(m_Move, crouch, jump);
    }


    private void MouseMovement()
    {
        bool crouch = Input.GetKey(KeyCode.C);

        if (Input.GetMouseButton(0))
        {
            print("Cursor raycast hit layer: " + cameraraycaster.layerHit);
            switch (cameraraycaster.layerHit)
            {
                case Layer.Enemy:
                    print("Clicked on Enemy");
                    break;
                case Layer.Walkable:
                    currentClickTarget = cameraraycaster.hit.point;
                    thirdPersonController.Move(currentClickTarget - transform.position, crouch, jump);
                    break;


                default:
                    print("Something is missing a layer");
                    break;
            }

            var playerToClickPoint = currentClickTarget - transform.position;
            if (playerToClickPoint.magnitude >= walkMoveStopRadius)
            {
                thirdPersonController.Move(playerToClickPoint, crouch, jump);
            }
            else
            {
                thirdPersonController.Move(Vector3.zero, crouch, jump);
            }
        }
    }
}