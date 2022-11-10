/*
 * CameraController -
 * Created by : Allan N. Murillo
 * Last Edited : 7/3/2021
 */

using Cinemachine;
using UnityEngine;
using ANM.Input;
using ANM.Scriptables.Manager;

namespace ANM.Control
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera defaultVirtualCamera;
        [SerializeField] private CinemachineVirtualCamera closeVirtualCamera;
        private InputController _input;
        
        public static CameraState currentState;
        
        //  TODO : have different camera behaviour states the player can toggle through
        //  TODO : use raycasts to move camera from obstacles blocking view of player
        //  TODO : Cinemachine should also change the follow Y offset when entering tunnels

        private void OnEnable()
        {
            _input = ResourcesManager.FindResource<InputController>("PlayerControls");
            SetState(CameraState.Default);
            // _input.OnCameraToggleEvent += OnCameraToggleEvent;
        }

        private void OnCameraToggleEvent()
        {
            ToggleState();
        }

        private void OnDisable()
        {
            // _input.OnCameraToggleEvent -= OnCameraToggleEvent;
        }

        private static void SetState(CameraState state) => currentState = state;

        //  TODO : totem pole vcam priority is set to : 10
        private void ToggleState()
        {
            ChangeDown();
            switch (currentState)
            {
                case CameraState.Close:
                    defaultVirtualCamera.Priority = 15;
                    closeVirtualCamera.Priority = 20;
                    break;
                default:
                    defaultVirtualCamera.Priority = 20;
                    closeVirtualCamera.Priority = 15;
                    break;
            }
        }

        private static void ChangeUp()
        {
            var cur = (int) currentState;
            cur++;
            if (cur > (int) CameraState.Close) SetState(CameraState.Default);
            else currentState = (CameraState) cur;
        }   //  Right or Up

        private static void ChangeDown()
        {
            var cur = (int) currentState;
            cur--;
            if (cur < 0) SetState(CameraState.Close);
            else currentState = (CameraState) cur;
        }   //  Left or Down
    }

    public enum CameraState
    {
        Default,    //  lets cinemachine do its default thing
        Close,      //  follow bomb arrow proto series
    }
}
