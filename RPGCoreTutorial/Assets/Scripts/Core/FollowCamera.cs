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