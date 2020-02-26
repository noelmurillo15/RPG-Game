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
        private void LateUpdate()
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}