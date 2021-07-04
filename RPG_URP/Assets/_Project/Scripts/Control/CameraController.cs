/*
 * CameraController -
 * Created by : Allan N. Murillo
 * Last Edited : 7/3/2021
 */

using Cinemachine;
using UnityEngine;

namespace ANM.Control
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera defaultVirtualCamera;
        [SerializeField] private CinemachineVirtualCamera closeVirtualCamera;
        
        public static CameraState currentState;
        
        //  TODO : have different camera behaviour states the player can toggle through
        //  TODO : use raycasts to move camera from obstacles blocking view of player
        //  TODO : Cinemachine should also change the follow Y offset when entering tunnels

        private void OnEnable()
        {
            SetState(CameraState.Default);
        }

        public static void SetState(CameraState state) => currentState = state;

        public void ToggleState()
        {
            ChangeUp();
        }

        private static void ChangeUp()
        {
            var cur = (int) currentState;
            cur++;
            if (cur > (int) CameraState.Close) currentState = CameraState.Default;
            else currentState = (CameraState) cur;
        }   //  Right or Up

        private void ChangeDown()
        {
            var cur = (int) currentState;
            cur--;
            if (cur < 0) currentState = CameraState.Close;
            else currentState = (CameraState) cur;
        }   //  Left or Down
    }

    public enum CameraState
    {
        Default,    //  lets cinemachine do its default thing
        Close,      //  follow bomb arrow proto series
    }
}
