using UnityEngine;


namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] Transform target = null;


        void LateUpdate()
        {
            transform.localPosition = target.localPosition;
        }
    }
}