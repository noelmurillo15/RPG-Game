using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(ThirdPersonCharacter))]
public class Click2Move : MonoBehaviour
{
    [SerializeField] float walkMoveStopRadius = 0.2f;
    ThirdPersonCharacter m_Character;
    private CameraRaycaster cameraraycaster;
    Vector3 currentClickTarget;

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
        if (Input.GetMouseButton(0))
        {
            print("Cursor raycast hit layer: " + cameraraycaster.layerHit);
            switch (cameraraycaster.layerHit)
            {
                case Layer.RaycastEndStop:
                    break;
                case Layer.Enemy:
                    print("Clicked on Enemy");
                    break;
                case Layer.Walkable:
                    //currentClickTarget = cameraraycaster.hit.point;
                    //  For Click to Move Movement - Comment out line 70 in ThirdPersonUserControl.cs
                    //m_Character.Move(currentClickTarget - transform.position, false, false);
                    break;


                default:
                    print("Something is missing a layer");
                    break;
            }

            //var playerToClickPoint = currentClickTarget - transform.position;
            //if(playerToClickPoint.magnitude >= walkMoveStopRadius)
            //{
            //    m_Character.Move(playerToClickPoint, false, false);
            //}
            //else
            //{
            //    m_Character.Move(Vector3.zero, false, false);
            //}
        }
    }
}