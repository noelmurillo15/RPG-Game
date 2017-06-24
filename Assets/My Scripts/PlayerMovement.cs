using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float walkMoveStopRadius = 0.2f;
    ThirdPersonCharacter m_Character;
    private CameraRaycaster cameraraycaster;
    Vector3 currentClickTarget;
    bool InputType = false;    //  TODO consider making static layer

    // Use this for initialization
    void Start()
    {
        cameraraycaster = Camera.main.GetComponent<CameraRaycaster>();
        m_Character = GetComponent<ThirdPersonCharacter>();
        currentClickTarget = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.G))
            InputType = !InputType;

        if (InputType)
            MouseMovement();
        else
            GamepadMovement();
    }

    private void GamepadMovement()
    {

    //  Joystick Movement
        // read inputs
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // calculate camera relative direction to move:
        Vector3 m_CamForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 m_Move = v * m_CamForward + h * Camera.main.transform.right;

        m_Character.Move(m_Move, false, false);
    //  Joystick Movement
    }

    private void MouseMovement()
    {
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
                    m_Character.Move(currentClickTarget - transform.position, false, false);
                    break;


                default:
                    print("Something is missing a layer");
                    break;
            }

            var playerToClickPoint = currentClickTarget - transform.position;
            if (playerToClickPoint.magnitude >= walkMoveStopRadius)
            {
                m_Character.Move(playerToClickPoint, false, false);
            }
            else
            {
                m_Character.Move(Vector3.zero, false, false);
            }
        }
    }
}