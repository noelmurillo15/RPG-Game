/*
 * FollowCamera - 
 * Created by : Allan N. Murillo
 * Last Edited : 2/25/2020
 */

using UnityEngine;

namespace ANM.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] private Transform target = null;


        private void LateUpdate()
        {
            transform.localPosition = target.localPosition;
        }
    }
}