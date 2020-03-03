/*
 * UICameraFacing - 
 * Created by : Allan N. Murillo
 * Last Edited : 2/25/2020
 */

using UnityEngine;

namespace ANM.UI
{
    public class UICameraFacing : MonoBehaviour
    {
        private Camera _mainCam;

        private void Start()
        {
            _mainCam = Camera.main;
        }

        private void LateUpdate()
        {
            transform.forward = _mainCam.transform.forward;
        }
    }
}